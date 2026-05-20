// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;
using System.Net.Http.Json;
using Azure.AI.AgentServer.Core;
using Azure.AI.Projects;
using Azure.Identity;
using DotNetEnv;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry.Hosting;
using Microsoft.Extensions.AI;

Env.TraversePath().Load();

var projectEndpoint = new Uri(Environment.GetEnvironmentVariable("AZURE_AI_PROJECT_ENDPOINT")
    ?? throw new InvalidOperationException("AZURE_AI_PROJECT_ENDPOINT environment variable is not set."));
var deployment = Environment.GetEnvironmentVariable("AZURE_AI_MODEL_DEPLOYMENT_NAME") ?? "gpt-4o";

// Resolve the weather service URL. In Aspire it is injected via service discovery
// (services__weather-service__https__0 / __http__0); locally the WEATHER_SERVICE_URL
// env var (e.g. via .env) is the fallback.
var weatherServiceUrl = Environment.GetEnvironmentVariable("WEATHER_SERVICE_URL")
    ?? Environment.GetEnvironmentVariable("services__weather-service__https__0")
    ?? Environment.GetEnvironmentVariable("services__weather-service__http__0")
    ?? throw new InvalidOperationException(
        "Weather service URL is not set. Provide WEATHER_SERVICE_URL or run via the Aspire AppHost.");

var weatherClient = new HttpClient { BaseAddress = new Uri(weatherServiceUrl) };

// Tool exposed to the agent. The model picks candidate dates and calls this once per
// date; we return the raw forecast plus a derived classification per day so the model
// can reason about trade-offs.
[Description("Fetches the 5-day picnic forecast (E-2..E+2) centered on the given event date and classifies each day for picnic suitability.")]
async Task<ForecastToolResult> GetForecastAsync(
    [Description("The candidate picnic date in ISO format (YYYY-MM-DD).")] DateOnly eventDate,
    CancellationToken cancellationToken)
{
    var response = await weatherClient.GetFromJsonAsync<ForecastResponse>(
        $"/forecast?eventDate={eventDate:yyyy-MM-dd}",
        cancellationToken)
        ?? throw new InvalidOperationException("Weather service returned no forecast.");

    var classified = response.Days
        .Select(d => new ClassifiedDay(d.Date, d.TemperatureF, d.Condition, d.IsEventDay, WeatherClassifier.Classify(d.TemperatureF, d.Condition)))
        .ToArray();

    return new ForecastToolResult(response.EventDate, classified);
}

AIAgent agent = new AIProjectClient(projectEndpoint, new DefaultAzureCredential())
    .AsAIAgent(
        model: deployment,
        instructions: """
            You are the Weather Agent for Picnic In The Park.
            Your job is to help the caller pick a picnic day that is sunny and not too hot.

            Workflow:
            1. If the caller has not given you one or more candidate dates, ask for them
               (and a location, even though the forecast tool currently ignores it).
            2. For each candidate date, call the get_forecast tool exactly once.
            3. Prefer days classified "Ideal" (sunny and a comfortable temperature).
               If none are Ideal, fall back to "Acceptable" days and explain the trade-off
               (e.g. cloudy but mild, or warm but sunny). Avoid "Risk:Rain", "Risk:Heat",
               and "Unsafe" days unless the caller insists.
            4. Recommend one day, and briefly summarise the supporting forecast: the date,
               temperature in °F, condition, and classification.

            Be concise, factual, and never invent forecast data — only use what the tool returns.
            """,
        name: "weather-agent",
        description: "Picks a sunny, not-too-hot picnic day by consulting the picnic weather service.",
        tools: [AIFunctionFactory.Create(GetForecastAsync)]);

var builder = AgentHost.CreateBuilder(args);
builder.Services.AddFoundryResponses(agent);
builder.RegisterProtocol("responses", endpoints => endpoints.MapFoundryResponses());

var app = builder.Build();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureF, string Condition, bool IsEventDay);
internal record ForecastResponse(DateOnly EventDate, WeatherForecast[] Days);
internal record ClassifiedDay(DateOnly Date, int TemperatureF, string Condition, bool IsEventDay, string Classification);
internal record ForecastToolResult(DateOnly EventDate, ClassifiedDay[] Days);

/// <summary>
/// Maps a (temperature, condition) pair to the picnic risk taxonomy defined in PRD §7.2 (FR-W2):
/// Ideal, Acceptable, Risk:Rain, Risk:Heat, Unsafe.
/// </summary>
/// <remarks>
/// Thresholds (°F): Ideal = Sunny and 65–80; Risk:Heat = > 85 (any condition);
/// Risk:Rain = Rain (any temperature, unless already Risk:Heat); Acceptable = Cloudy,
/// or Sunny in the 80–85 band, or mild conditions outside the Ideal window.
/// "Unsafe" is reserved for severe-weather extensions and is not currently emitted by
/// the seeded weather service.
/// </remarks>
internal static class WeatherClassifier
{
    public static string Classify(int temperatureF, string condition)
    {
        if (temperatureF > 85) return "Risk:Heat";
        if (string.Equals(condition, "Rain", StringComparison.OrdinalIgnoreCase)) return "Risk:Rain";
        if (string.Equals(condition, "Sunny", StringComparison.OrdinalIgnoreCase)
            && temperatureF is >= 65 and <= 80)
        {
            return "Ideal";
        }
        return "Acceptable";
    }
}

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

string[] conditions = ["Sunny", "Cloudy", "Rain"];

app.MapGet("/forecast", (DateOnly? eventDate) =>
{
    if (eventDate is null)
        return Results.BadRequest("eventDate query parameter is required (format: YYYY-MM-DD).");

    var date = eventDate.Value;
    var days = Enumerable.Range(-2, 5).Select(offset =>
    {
        var day = date.AddDays(offset);
        return new WeatherForecast(
            day,
            Random.Shared.Next(65, 86),
            conditions[Random.Shared.Next(conditions.Length)],
            day == date);
    }).ToArray();

    return Results.Ok(new ForecastResponse(date, days));
})
.WithName("GetPicnicForecast")
.WithSummary("Returns a 5-day forecast centered on the given event date (E-2 … E+2).");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureF, string Condition, bool IsEventDay);
record ForecastResponse(DateOnly EventDate, WeatherForecast[] Days);

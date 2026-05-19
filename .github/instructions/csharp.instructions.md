---
description: "Use when writing, modifying, or generating C# code. Covers C# conventions, DTO patterns, minimal API style, and package management."
applyTo: "**/*.cs"
---
# C# Conventions

## DTOs

Use `record` types for all DTOs and data transfer objects. Never use classes with mutable properties for DTOs.

```csharp
// Good
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);

// Bad — don't use mutable classes for DTOs
public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
```

## Minimal API

Use minimal API style (`app.MapGet`, `app.MapPost`, etc.) instead of controllers. Organize endpoints using extension methods or route groups.

```csharp
app.MapGet("/forecasts", () => Results.Ok(forecasts));
app.MapPost("/forecasts", (WeatherForecast forecast) => { /* ... */ });
```

## Package Management

Always use `dotnet add package <PackageName>` to add NuGet packages. Never edit `.csproj` files directly to add or modify package references.

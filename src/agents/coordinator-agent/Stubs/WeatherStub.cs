namespace CoordinatorAgent.Stubs;

public static class WeatherStub
{
    private static readonly string[] Conditions = ["Sunny", "Partly Cloudy", "Cloudy", "Rain"];
    private static readonly string[] RiskLevels = ["Ideal", "Acceptable", "Risk:Rain", "Risk:Heat"];

    public static Task<WeatherResult> GetForecastAsync(DateOnly eventDate)
    {
        var rng = new Random(eventDate.DayNumber);
        var tempF = rng.Next(65, 88);
        var conditionIndex = rng.Next(Conditions.Length);
        var condition = Conditions[conditionIndex];

        var risk = condition switch
        {
            "Sunny" when tempF > 85 => "Risk:Heat",
            "Sunny" => "Ideal",
            "Partly Cloudy" => "Ideal",
            "Cloudy" => "Acceptable",
            "Rain" => "Risk:Rain",
            _ => "Acceptable"
        };

        return Task.FromResult(new WeatherResult(eventDate, tempF, condition, risk));
    }
}

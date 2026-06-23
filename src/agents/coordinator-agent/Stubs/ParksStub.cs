namespace CoordinatorAgent.Stubs;

public static class ParksStub
{
    public static Task<IReadOnlyList<ParkResult>> GetRecommendationsAsync(
        string locationPreference,
        string riskClassification)
    {
        var hasCoverBonus = riskClassification is "Risk:Rain" or "Risk:Heat";

        IReadOnlyList<ParkResult> parks =
        [
            new("park-1", "Lake View Park", "Springfield", "Lakeshore Pavilion", 50,
                hasCoverBonus ? 0.95m : 0.88m),
            new("park-1", "Lake View Park", "Springfield", "Oak Grove Shelter", 30,
                hasCoverBonus ? 0.90m : 0.82m),
            new("park-2", "Riverside Commons", "Springfield", "River Bend Gazebo", 40,
                hasCoverBonus ? 0.85m : 0.91m),
            new("park-3", "Hilltop Reserve", "Shelbyville", "Summit Shelter", 60,
                hasCoverBonus ? 0.92m : 0.78m),
        ];

        return Task.FromResult(parks);
    }
}
namespace CoordinatorAgent.Stubs;

public static class MenuStub
{
    public static Task<MenuResult> PlanMenuAsync(
        int partySize,
        decimal budget,
        string? dietaryNotes,
        string riskClassification)
    {
        var isRainy = riskClassification == "Risk:Rain";

        List<MenuItem> items =
        [
            new("Grilled Chicken Sandwiches", "Main", 3.50m * partySize),
            new(isRainy ? "Hot Tomato Soup" : "Caesar Salad", "Side", 2.00m * partySize),
            new("Fresh Fruit Platter", "Side", 1.75m * partySize),
            new(isRainy ? "Hot Chocolate" : "Lemonade", "Beverage", 1.25m * partySize),
            new("Chocolate Brownies", "Dessert", 1.50m * partySize),
        ];

        if (dietaryNotes?.Contains("vegetarian", StringComparison.OrdinalIgnoreCase) == true)
        {
            items[0] = new("Grilled Veggie Wraps", "Main", 3.25m * partySize);
        }

        var total = items.Sum(i => i.EstimatedCost);
        return Task.FromResult(new MenuResult(items, total));
    }
}
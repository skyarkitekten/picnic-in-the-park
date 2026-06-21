namespace CoordinatorAgent.Stubs;

public static class GroceryStub
{
    public static Task<GroceryResult> GetShoppingListAsync(MenuResult menu)
    {
        List<GroceryItem> items = [];

        foreach (var menuItem in menu.Items)
        {
            items.AddRange(menuItem.Category switch
            {
                "Main" =>
                [
                    new(menuItem.Name + " ingredients", "1 pack", menuItem.EstimatedCost * 0.7m, true),
                    new("Bread rolls", "1 bag", menuItem.EstimatedCost * 0.3m, true),
                ],
                "Side" =>
                [
                    new(menuItem.Name + " ingredients", "1 bag", menuItem.EstimatedCost * 0.8m, true),
                ],
                "Beverage" =>
                [
                    new(menuItem.Name, "1 jug / pack", menuItem.EstimatedCost, true),
                ],
                "Dessert" =>
                [
                    new(menuItem.Name + " mix", "1 box", menuItem.EstimatedCost * 0.6m, true),
                    new("Butter & eggs", "1 set", menuItem.EstimatedCost * 0.4m, true),
                ],
                _ =>
                [
                    new(menuItem.Name, "1 unit", menuItem.EstimatedCost, true),
                ],
            });
        }

        var total = items.Sum(i => i.Price);
        return Task.FromResult(new GroceryResult(items, total));
    }
}
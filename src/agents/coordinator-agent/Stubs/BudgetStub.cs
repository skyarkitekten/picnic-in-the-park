namespace CoordinatorAgent.Stubs;

public static class BudgetStub
{
    public static Task<BudgetSummary> CalculateAsync(
        decimal totalBudget,
        GroceryResult grocery,
        ReservationHold reservation)
    {
        var foodCost = grocery.TotalCost;
        var reservationCost = reservation.Status == "Held" ? 15.00m : 0m;
        var miscCost = 5.00m;
        var total = foodCost + reservationCost + miscCost;
        var remaining = totalBudget - total;

        return Task.FromResult(new BudgetSummary(foodCost, reservationCost, miscCost, total, remaining));
    }
}
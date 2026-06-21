namespace CoordinatorAgent;

// ── Request ──
public record PicnicPlanRequest(
    DateOnly EventDate,
    int PartySize,
    decimal Budget,
    string LocationPreference,
    string? DietaryNotes);

// ── Weather ──
public record WeatherResult(
    DateOnly Date,
    int TemperatureF,
    string Condition,
    string RiskClassification);

// ── Parks / Activity ──
public record ParkResult(
    string ParkId,
    string ParkName,
    string City,
    string ShelterName,
    int Capacity,
    decimal Score);

// ── Menu ──
public record MenuItem(string Name, string Category, decimal EstimatedCost);

public record MenuResult(IReadOnlyList<MenuItem> Items, decimal TotalCost);

// ── Grocery ──
public record GroceryItem(string Name, string Quantity, decimal Price, bool Available);

public record GroceryResult(IReadOnlyList<GroceryItem> Items, decimal TotalCost);

// ── Reservation ──
public record ReservationHold(
    string ParkName,
    string ShelterName,
    DateOnly Date,
    string Status);

// ── Budget ──
public record BudgetSummary(
    decimal FoodCost,
    decimal ReservationCost,
    decimal MiscCost,
    decimal Total,
    decimal Remaining);

// ── Assembled plan ──
public record PicnicPlan(
    string PlanId,
    PicnicPlanRequest Request,
    WeatherResult Weather,
    IReadOnlyList<ParkResult> Parks,
    MenuResult Menu,
    GroceryResult Grocery,
    ReservationHold Reservation,
    BudgetSummary Budget,
    string Status);

// ── Card wrapper for frontend rendering ──
public record AgentCard(string AgentName, string CardType, object Data);
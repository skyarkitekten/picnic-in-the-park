using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddSingleton(SeedData.Parks);
builder.Services.AddSingleton<ConcurrentDictionary<string, Reservation>>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ── Parks ────────────────────────────────────────────────────────────────────

app.MapGet("/parks", (IReadOnlyList<Park> parks) => Results.Ok(parks))
    .WithName("ListParks")
    .WithSummary("Returns all parks with their shelters.");

app.MapGet("/parks/{parkId}", (string parkId, IReadOnlyList<Park> parks) =>
{
    var park = parks.FirstOrDefault(p => p.Id == parkId);
    return park is null ? Results.NotFound() : Results.Ok(park);
})
.WithName("GetPark")
.WithSummary("Returns a single park by ID.");

// ── Reservations ─────────────────────────────────────────────────────────────

app.MapPost("/parks/{parkId}/shelters/{shelterId}/reserve",
    (string parkId, string shelterId, ReservationRequest body,
     IReadOnlyList<Park> parks, ConcurrentDictionary<string, Reservation> store) =>
{
    var park = parks.FirstOrDefault(p => p.Id == parkId);
    if (park is null) return Results.NotFound($"Park '{parkId}' not found.");

    var shelter = park.Shelters.FirstOrDefault(s => s.Id == shelterId);
    if (shelter is null) return Results.NotFound($"Shelter '{shelterId}' not found in park '{parkId}'.");

    var storeKey = $"{parkId}|{shelterId}|{body.Date:yyyy-MM-dd}";

    if (store.ContainsKey(storeKey))
        return Results.Conflict(new ReservationResult(false, null, "Shelter already reserved.", null));

    if (Random.Shared.Next(3) == 0)
        return Results.Ok(new ReservationResult(false, null, "All shelters booked.", null));

    var reservation = new Reservation(
        Guid.NewGuid().ToString("N"),
        parkId, shelterId,
        body.Date,
        body.ReservedBy);

    store[storeKey] = reservation;

    return Results.Ok(new ReservationResult(true, reservation.Id, null, reservation));
})
.WithName("ReserveShelter")
.WithSummary("Attempts to reserve a shelter. Returns success or reason for failure.");

app.MapGet("/reservations", (ConcurrentDictionary<string, Reservation> store) =>
    Results.Ok(store.Values.ToArray()))
    .WithName("ListReservations")
    .WithSummary("Lists all active reservations (demo/debug).");

app.MapDelete("/reservations/{id}", (string id, ConcurrentDictionary<string, Reservation> store) =>
{
    var entry = store.FirstOrDefault(kv => kv.Value.Id == id);
    if (entry.Key is null) return Results.NotFound();
    store.TryRemove(entry.Key, out _);
    return Results.NoContent();
})
.WithName("CancelReservation")
.WithSummary("Cancels a reservation by ID.");

app.Run();

// ── DTOs ─────────────────────────────────────────────────────────────────────

record Park(string Id, string Name, string City, IReadOnlyList<Shelter> Shelters);
record Shelter(string Id, string Name, int Capacity);
record ReservationRequest(DateOnly Date, string ReservedBy);
record Reservation(string Id, string ParkId, string ShelterId, DateOnly Date, string ReservedBy);
record ReservationResult(bool Success, string? ReservationId, string? Reason, Reservation? Reservation);

// ── Seed data ─────────────────────────────────────────────────────────────────

static class SeedData
{
    public static readonly IReadOnlyList<Park> Parks =
    [
        new Park("lake-view", "Lake View Park", "Springfield",
        [
            new Shelter("lv-s1", "Lakeshore Pavilion", 50),
            new Shelter("lv-s2", "Oak Grove Shelter", 30),
            new Shelter("lv-s3", "Willowbrook Canopy", 20),
        ]),
        new Park("riverside", "Riverside Commons", "Springfield",
        [
            new Shelter("rv-s1", "River Bend Gazebo", 40),
            new Shelter("rv-s2", "Elm Terrace", 25),
        ]),
        new Park("hilltop", "Hilltop Reserve", "Shelbyville",
        [
            new Shelter("ht-s1", "Summit Shelter", 60),
            new Shelter("ht-s2", "Meadow Pavilion", 35),
            new Shelter("ht-s3", "Cedar Ridge Canopy", 20),
        ]),
    ];
}
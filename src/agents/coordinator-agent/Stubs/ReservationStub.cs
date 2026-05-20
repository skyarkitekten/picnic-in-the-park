namespace CoordinatorAgent.Stubs;

public static class ReservationStub
{
    public static Task<ReservationHold> HoldShelterAsync(
        string parkName,
        string shelterName,
        DateOnly date)
    {
        return Task.FromResult(new ReservationHold(parkName, shelterName, date, "Held"));
    }
}

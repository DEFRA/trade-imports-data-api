namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ApplicantConservationOfSample
{
    public const string Ambient = "Ambient";
    public const string Chilled = "Chilled";
    public const string Frozen = "Frozen";

    public static bool IsAmbient(string? status) => Equals(Ambient, status);

    public static bool IsChilled(string? status) => Equals(Chilled, status);

    public static bool IsFrozen(string? status) => Equals(Frozen, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionDefinitiveImportPurpose
{
    public const string Slaughter = "slaughter";
    public const string ApprovedBodies = "approvedbodies";
    public const string Quarantine = "quarantine";

    public static bool IsSlaughter(string? status) => Equals(Slaughter, status);

    public static bool IsApprovedBodies(string? status) => Equals(ApprovedBodies, status);

    public static bool IsQuarantine(string? status) => Equals(Quarantine, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

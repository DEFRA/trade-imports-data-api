namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ConsignmentCheckIdentityCheckNotDoneReason
{
    public const string ReducedChecksRegime = "Reduced checks regime";
    public const string NotRequired = "Not required";

    public static bool IsReducedChecksRegime(string? status) => Equals(ReducedChecksRegime, status);

    public static bool IsNotRequired(string? status) => Equals(NotRequired, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

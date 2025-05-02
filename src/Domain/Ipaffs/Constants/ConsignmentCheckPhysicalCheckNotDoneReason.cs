namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ConsignmentCheckPhysicalCheckNotDoneReason
{
    public const string ReducedChecksRegime = "Reduced checks regime";
    public const string Other = "Other";

    public static bool IsReducedChecksRegime(string? status) => Equals(ReducedChecksRegime, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

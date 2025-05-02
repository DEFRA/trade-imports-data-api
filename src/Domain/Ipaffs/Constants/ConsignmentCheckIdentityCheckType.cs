namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ConsignmentCheckIdentityCheckType
{
    public const string SealCheck = "Seal Check";
    public const string FullIdentityCheck = "Full Identity Check";
    public const string NotDone = "Not Done";

    public static bool IsSealCheck(string? status) => Equals(SealCheck, status);

    public static bool IsFullIdentityCheck(string? status) => Equals(FullIdentityCheck, status);

    public static bool IsNotDone(string? status) => Equals(NotDone, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class SealCheckOverrideOriginalDecision
{
    public const string Required = "Required";
    public const string Inconclusive = "Inconclusive";
    public const string NotRequired = "Not required";

    public static bool IsRequired(string? status) => Equals(Required, status);

    public static bool IsInconclusive(string? status) => Equals(Inconclusive, status);

    public static bool IsNotRequired(string? status) => Equals(NotRequired, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

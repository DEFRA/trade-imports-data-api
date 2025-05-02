namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class CommodityRiskResultRiskDecision
{
    public const string Required = "REQUIRED";
    public const string NotRequired = "NOTREQUIRED";
    public const string Inconclusive = "INCONCLUSIVE";
    public const string ReEnforcedCheck = "REENFORCED_CHECK";

    public static bool IsRequired(string? status) => Equals(Required, status);

    public static bool IsNotRequired(string? status) => Equals(NotRequired, status);

    public static bool IsInconclusive(string? status) => Equals(Inconclusive, status);

    public static bool IsReEnforcedCheck(string? status) => Equals(ReEnforcedCheck, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

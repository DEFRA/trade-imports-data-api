namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class CommodityResultsRiskDecision
{
    public const string Required = "REQUIRED";
    public const string Notrequired = "NOTREQUIRED";
    public const string Inconclusive = "INCONCLUSIVE";
    public const string ReenforcedCheck = "REENFORCED_CHECK";

    public static bool IsRequired(string? status) => Equals(Required, status);

    public static bool IsNotrequired(string? status) => Equals(Notrequired, status);

    public static bool IsInconclusive(string? status) => Equals(Inconclusive, status);

    public static bool IsReenforcedCheck(string? status) => Equals(ReenforcedCheck, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

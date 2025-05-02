namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class CommodityRiskResultPhsiDecision
{
    public const string Required = "REQUIRED";
    public const string NotRequired = "NOTREQUIRED";

    public static bool IsRequired(string? status) => Equals(Required, status);

    public static bool IsNotRequired(string? status) => Equals(NotRequired, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

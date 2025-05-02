namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class CommodityResultsPhsiDecision
{
    public const string Required = "REQUIRED";
    public const string Notrequired = "NOTREQUIRED";

    public static bool IsRequired(string? status) => Equals(Required, status);

    public static bool IsNotrequired(string? status) => Equals(Notrequired, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

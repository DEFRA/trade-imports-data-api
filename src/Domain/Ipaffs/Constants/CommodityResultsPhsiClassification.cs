namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class CommodityResultsPhsiClassification
{
    public const string Mandatory = "Mandatory";
    public const string Reduced = "Reduced";
    public const string Controlled = "Controlled";

    public static bool IsMandatory(string? status) => Equals(Mandatory, status);

    public static bool IsReduced(string? status) => Equals(Reduced, status);

    public static bool IsControlled(string? status) => Equals(Controlled, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

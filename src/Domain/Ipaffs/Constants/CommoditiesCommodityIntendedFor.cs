namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class CommoditiesCommodityIntendedFor
{
    public const string Human = "human";
    public const string Feedingstuff = "feedingstuff";
    public const string Further = "further";
    public const string Other = "other";

    public static bool IsHuman(string? status) => Equals(Human, status);

    public static bool IsFeedingstuff(string? status) => Equals(Feedingstuff, status);

    public static bool IsFurther(string? status) => Equals(Further, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

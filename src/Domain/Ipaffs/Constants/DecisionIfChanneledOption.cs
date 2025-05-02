namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionIfChanneledOption
{
    public const string Article8 = "article8";
    public const string Article15 = "article15";

    public static bool IsArticle8(string? status) => Equals(Article8, status);

    public static bool IsArticle15(string? status) => Equals(Article15, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class JourneyRiskCategorisationResultRiskLevel
{
    public const string High = "High";
    public const string Medium = "Medium";
    public const string Low = "Low";

    public static bool IsHigh(string? status) => Equals(High, status);

    public static bool IsMedium(string? status) => Equals(Medium, status);

    public static bool IsLow(string? status) => Equals(Low, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

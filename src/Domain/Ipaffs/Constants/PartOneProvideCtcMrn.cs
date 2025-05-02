namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PartOneProvideCtcMrn
{
    public const string Yes = "YES";
    public const string YesAddLater = "YES_ADD_LATER";
    public const string No = "NO";

    public static bool IsYes(string? status) => Equals(Yes, status);

    public static bool IsYesAddLater(string? status) => Equals(YesAddLater, status);

    public static bool IsNo(string? status) => Equals(No, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

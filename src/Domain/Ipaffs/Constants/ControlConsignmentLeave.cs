namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ControlConsignmentLeave
{
    public const string Yes = "YES";
    public const string No = "NO";
    public const string ItHasBeenDestroyed = "It has been destroyed";

    public static bool IsYes(string? status) => Equals(Yes, status);

    public static bool IsNo(string? status) => Equals(No, status);

    public static bool IsItHasBeenDestroyed(string? status) => Equals(ItHasBeenDestroyed, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ConsignorStatus
{
    public const string Approved = "approved";
    public const string Nonapproved = "nonapproved";
    public const string Suspended = "suspended";

    public static bool IsApproved(string? status) => Equals(Approved, status);

    public static bool IsNonapproved(string? status) => Equals(Nonapproved, status);

    public static bool IsSuspended(string? status) => Equals(Suspended, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

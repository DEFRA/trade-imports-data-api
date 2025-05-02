namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class FeedbackInformationAuthorityType
{
    public const string Exitbip = "exitbip";
    public const string Finalbip = "finalbip";
    public const string Localvetunit = "localvetunit";
    public const string Inspunit = "inspunit";

    public static bool IsExitbip(string? status) => Equals(Exitbip, status);

    public static bool IsFinalbip(string? status) => Equals(Finalbip, status);

    public static bool IsLocalvetunit(string? status) => Equals(Localvetunit, status);

    public static bool IsInspunit(string? status) => Equals(Inspunit, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

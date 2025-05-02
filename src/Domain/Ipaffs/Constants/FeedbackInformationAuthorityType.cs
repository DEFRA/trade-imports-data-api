namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class FeedbackInformationAuthorityType
{
    public const string ExitBip = "exitbip";
    public const string FinalBip = "finalbip";
    public const string LocalVetUnit = "localvetunit";
    public const string InspUnit = "inspunit";

    public static bool IsExitBip(string? status) => Equals(ExitBip, status);

    public static bool IsFinalBip(string? status) => Equals(FinalBip, status);

    public static bool IsLocalVetUnit(string? status) => Equals(LocalVetUnit, status);

    public static bool IsInspUnit(string? status) => Equals(InspUnit, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

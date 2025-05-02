namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ConsignmentCheckEuStandard
{
    public const string Satisfactory = "Satisfactory";
    public const string SatisfactoryFollowingOfficialIntervention = "Satisfactory following official intervention";
    public const string NotSatisfactory = "Not Satisfactory";
    public const string NotDone = "Not Done";
    public const string Derogation = "Derogation";
    public const string NotSet = "Not Set";

    public static bool IsSatisfactory(string? status) => Equals(Satisfactory, status);

    public static bool IsSatisfactoryFollowingOfficialIntervention(string? status) =>
        Equals(SatisfactoryFollowingOfficialIntervention, status);

    public static bool IsNotSatisfactory(string? status) => Equals(NotSatisfactory, status);

    public static bool IsNotDone(string? status) => Equals(NotDone, status);

    public static bool IsDerogation(string? status) => Equals(Derogation, status);

    public static bool IsNotSet(string? status) => Equals(NotSet, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

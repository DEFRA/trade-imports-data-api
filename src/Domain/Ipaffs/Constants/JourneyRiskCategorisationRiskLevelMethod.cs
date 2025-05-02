namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class JourneyRiskCategorisationRiskLevelMethod
{
    public const string System = "System";
    public const string User = "User";

    public static bool IsSystem(string? status) => Equals(System, status);

    public static bool IsUser(string? status) => Equals(User, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class LaboratoryTestsTestReason
{
    public const string Random = "Random";
    public const string Suspicious = "Suspicious";
    public const string ReEnforced = "Re-enforced";
    public const string IntensifiedControls = "Intensified controls";
    public const string Required = "Required";
    public const string LatentInfectionSampling = "Latent infection sampling";

    public static bool IsRandom(string? status) => Equals(Random, status);

    public static bool IsSuspicious(string? status) => Equals(Suspicious, status);

    public static bool IsReEnforced(string? status) => Equals(ReEnforced, status);

    public static bool IsIntensifiedControls(string? status) => Equals(IntensifiedControls, status);

    public static bool IsRequired(string? status) => Equals(Required, status);

    public static bool IsLatentInfectionSampling(string? status) => Equals(LatentInfectionSampling, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ApplicantAnalysisType
{
    public const string InitialAnalysis = "Initial analysis";
    public const string CounterAnalysis = "Counter analysis";
    public const string SecondExpertAnalysis = "Second expert analysis";

    public static bool IsInitialAnalysis(string? status) => Equals(InitialAnalysis, status);

    public static bool IsCounterAnalysis(string? status) => Equals(CounterAnalysis, status);

    public static bool IsSecondExpertAnalysis(string? status) => Equals(SecondExpertAnalysis, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

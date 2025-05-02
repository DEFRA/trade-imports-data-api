namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class LaboratoryTestResultConclusion
{
    public const string Satisfactory = "Satisfactory";
    public const string NotSatisfactory = "Not satisfactory";
    public const string NotInterpretable = "Not interpretable";
    public const string Pending = "Pending";

    public static bool IsSatisfactory(string? status) => Equals(Satisfactory, status);

    public static bool IsNotSatisfactory(string? status) => Equals(NotSatisfactory, status);

    public static bool IsNotInterpretable(string? status) => Equals(NotInterpretable, status);

    public static bool IsPending(string? status) => Equals(Pending, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

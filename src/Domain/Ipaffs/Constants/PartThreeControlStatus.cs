namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PartThreeControlStatus
{
    public const string Required = "REQUIRED";
    public const string Completed = "COMPLETED";

    public static bool IsRequired(string? status) => Equals(Required, status);

    public static bool IsCompleted(string? status) => Equals(Completed, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

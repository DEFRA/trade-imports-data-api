namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PartOneTypeOfImp
{
    public const string A = "A";
    public const string P = "P";
    public const string D = "D";

    public static bool IsA(string? status) => Equals(A, status);

    public static bool IsP(string? status) => Equals(P, status);

    public static bool IsD(string? status) => Equals(D, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

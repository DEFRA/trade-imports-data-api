namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ChedppNotAcceptableReasonCommodityOrPackage
{
    public const string C = "c";
    public const string P = "p";
    public const string Cp = "cp";

    public static bool IsC(string? status) => Equals(C, status);

    public static bool IsP(string? status) => Equals(P, status);

    public static bool IsCp(string? status) => Equals(Cp, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

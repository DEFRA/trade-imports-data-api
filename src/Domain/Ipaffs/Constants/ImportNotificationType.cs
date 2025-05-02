namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ImportNotificationType
{
    public const string Cveda = "CVEDA";
    public const string Cvedp = "CVEDP";
    public const string Chedpp = "CHEDPP";
    public const string Ced = "CED";
    public const string Imp = "IMP";

    public static bool IsCveda(string? status) => Equals(Cveda, status);

    public static bool IsCvedp(string? status) => Equals(Cvedp, status);

    public static bool IsChedpp(string? status) => Equals(Chedpp, status);

    public static bool IsCed(string? status) => Equals(Ced, status);

    public static bool IsImp(string? status) => Equals(Imp, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

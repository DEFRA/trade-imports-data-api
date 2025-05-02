namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ControlAuthorityIuuOption
{
    public const string IUUOK = "IUUOK";
    public const string IUUNA = "IUUNA";
    public const string IUUNotCompliant = "IUUNotCompliant";

    public static bool IsIUUOK(string? status) => Equals(IUUOK, status);

    public static bool IsIUUNA(string? status) => Equals(IUUNA, status);

    public static bool IsIUUNotCompliant(string? status) => Equals(IUUNotCompliant, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

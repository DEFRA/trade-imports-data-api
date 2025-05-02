namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ImpactOfTransportOnAnimalsNumberOfDeadAnimalsUnit
{
    public const string Percent = "percent";
    public const string Number = "number";

    public static bool IsPercent(string? status) => Equals(Percent, status);

    public static bool IsNumber(string? status) => Equals(Number, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

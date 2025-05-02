namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PersonResponsibleType
{
    public const string CommercialTransporter = "Commercial transporter";
    public const string PrivateTransporter = "Private transporter";

    public static bool IsCommercialTransporter(string? status) => Equals(CommercialTransporter, status);

    public static bool IsPrivateTransporter(string? status) => Equals(PrivateTransporter, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

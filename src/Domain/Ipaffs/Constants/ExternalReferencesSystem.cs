namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ExternalReferencesSystem
{
    public const string ECert = "E-CERT";
    public const string EPhyto = "E-PHYTO";
    public const string ENotification = "E-NOTIFICATION";
    public const string Ncts = "NCTS";

    public static bool IsECert(string? status) => Equals(ECert, status);

    public static bool IsEPhyto(string? status) => Equals(EPhyto, status);

    public static bool IsENotification(string? status) => Equals(ENotification, status);

    public static bool IsNcts(string? status) => Equals(Ncts, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

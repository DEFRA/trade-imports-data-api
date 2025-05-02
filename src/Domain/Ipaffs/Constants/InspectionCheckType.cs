namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class InspectionCheckType
{
    public const string PhsiDocument = "PHSI_DOCUMENT";
    public const string PhsiIdentity = "PHSI_IDENTITY";
    public const string PhsiPhysical = "PHSI_PHYSICAL";
    public const string Hmi = "HMI";

    public static bool IsPhsiDocument(string? status) => Equals(PhsiDocument, status);

    public static bool IsPhsiIdentity(string? status) => Equals(PhsiIdentity, status);

    public static bool IsPhsiPhysical(string? status) => Equals(PhsiPhysical, status);

    public static bool IsHmi(string? status) => Equals(Hmi, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PlaceOfDestinationType
{
    public const string Consignee = "consignee";
    public const string Destination = "destination";
    public const string Exporter = "exporter";
    public const string Importer = "importer";
    public const string Charity = "charity";
    public const string CommercialTransporter = "commercial transporter";
    public const string CommercialTransporterUserAdded = "commercial transporter - user added";
    public const string PrivateTransporter = "private transporter";
    public const string TemporaryAddress = "temporary address";
    public const string PremisesOfOrigin = "premises of origin";
    public const string OrganisationBranchAddress = "organisation branch address";
    public const string Packer = "packer";
    public const string Pod = "pod";

    public static bool IsConsignee(string? status) => Equals(Consignee, status);

    public static bool IsDestination(string? status) => Equals(Destination, status);

    public static bool IsExporter(string? status) => Equals(Exporter, status);

    public static bool IsImporter(string? status) => Equals(Importer, status);

    public static bool IsCharity(string? status) => Equals(Charity, status);

    public static bool IsCommercialTransporter(string? status) => Equals(CommercialTransporter, status);

    public static bool IsCommercialTransporterUserAdded(string? status) =>
        Equals(CommercialTransporterUserAdded, status);

    public static bool IsPrivateTransporter(string? status) => Equals(PrivateTransporter, status);

    public static bool IsTemporaryAddress(string? status) => Equals(TemporaryAddress, status);

    public static bool IsPremisesOfOrigin(string? status) => Equals(PremisesOfOrigin, status);

    public static bool IsOrganisationBranchAddress(string? status) => Equals(OrganisationBranchAddress, status);

    public static bool IsPacker(string? status) => Equals(Packer, status);

    public static bool IsPod(string? status) => Equals(Pod, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

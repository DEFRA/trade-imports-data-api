namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PurposePurposeGroup
{
    public const string ForImport = "For Import";
    public const string ForNonConformingConsignments = "For NON-Conforming Consignments";
    public const string ForTranshipmentTo = "For Transhipment to";
    public const string ForTransitTo3rdCountry = "For Transit to 3rd Country";
    public const string ForReImport = "For Re-Import";
    public const string ForPrivateImport = "For Private Import";
    public const string ForTransferTo = "For Transfer To";
    public const string ForImportReConformityCheck = "For Import Re-Conformity Check";

    public static bool IsForImport(string? status) => Equals(ForImport, status);

    public static bool IsForNonConformingConsignments(string? status) => Equals(ForNonConformingConsignments, status);

    public static bool IsForTranshipmentTo(string? status) => Equals(ForTranshipmentTo, status);

    public static bool IsForTransitTo3rdCountry(string? status) => Equals(ForTransitTo3rdCountry, status);

    public static bool IsForReImport(string? status) => Equals(ForReImport, status);

    public static bool IsForPrivateImport(string? status) => Equals(ForPrivateImport, status);

    public static bool IsForTransferTo(string? status) => Equals(ForTransferTo, status);

    public static bool IsForImportReConformityCheck(string? status) => Equals(ForImportReConformityCheck, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

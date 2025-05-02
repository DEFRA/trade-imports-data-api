namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class AccompanyingDocumentsDocumentType
{
    public const string AirWaybill = "airWaybill";
    public const string BillOfLading = "billOfLading";
    public const string CargoManifest = "cargoManifest";
    public const string CatchCertificate = "catchCertificate";
    public const string CommercialDocument = "commercialDocument";
    public const string CommercialInvoice = "commercialInvoice";
    public const string ConformityCertificate = "conformityCertificate";
    public const string ContainerManifest = "containerManifest";
    public const string CustomsDeclaration = "customsDeclaration";
    public const string Docom = "docom";
    public const string HealthCertificate = "healthCertificate";
    public const string HeatTreatmentCertificate = "heatTreatmentCertificate";
    public const string ImportPermit = "importPermit";
    public const string InspectionCertificate = "inspectionCertificate";
    public const string Itahc = "itahc";
    public const string JourneyLog = "journeyLog";
    public const string LaboratorySamplingResultsForAflatoxin = "laboratorySamplingResultsForAflatoxin";
    public const string LatestVeterinaryHealthCertificate = "latestVeterinaryHealthCertificate";
    public const string LetterOfAuthority = "letterOfAuthority";
    public const string LicenseOrAuthorisation = "licenseOrAuthorisation";
    public const string MycotoxinCertification = "mycotoxinCertification";
    public const string OriginCertificate = "originCertificate";
    public const string Other = "other";
    public const string PhytosanitaryCertificate = "phytosanitaryCertificate";
    public const string ProcessingStatement = "processingStatement";
    public const string ProofOfStorage = "proofOfStorage";
    public const string RailwayBill = "railwayBill";
    public const string SeaWaybill = "seaWaybill";
    public const string VeterinaryHealthCertificate = "veterinaryHealthCertificate";
    public const string ListOfIngredients = "listOfIngredients";
    public const string PackingList = "packingList";
    public const string RoadConsignmentNote = "roadConsignmentNote";

    public static bool IsAirWaybill(string? status) => Equals(AirWaybill, status);

    public static bool IsBillOfLading(string? status) => Equals(BillOfLading, status);

    public static bool IsCargoManifest(string? status) => Equals(CargoManifest, status);

    public static bool IsCatchCertificate(string? status) => Equals(CatchCertificate, status);

    public static bool IsCommercialDocument(string? status) => Equals(CommercialDocument, status);

    public static bool IsCommercialInvoice(string? status) => Equals(CommercialInvoice, status);

    public static bool IsConformityCertificate(string? status) => Equals(ConformityCertificate, status);

    public static bool IsContainerManifest(string? status) => Equals(ContainerManifest, status);

    public static bool IsCustomsDeclaration(string? status) => Equals(CustomsDeclaration, status);

    public static bool IsDocom(string? status) => Equals(Docom, status);

    public static bool IsHealthCertificate(string? status) => Equals(HealthCertificate, status);

    public static bool IsHeatTreatmentCertificate(string? status) => Equals(HeatTreatmentCertificate, status);

    public static bool IsImportPermit(string? status) => Equals(ImportPermit, status);

    public static bool IsInspectionCertificate(string? status) => Equals(InspectionCertificate, status);

    public static bool IsItahc(string? status) => Equals(Itahc, status);

    public static bool IsJourneyLog(string? status) => Equals(JourneyLog, status);

    public static bool IsLaboratorySamplingResultsForAflatoxin(string? status) =>
        Equals(LaboratorySamplingResultsForAflatoxin, status);

    public static bool IsLatestVeterinaryHealthCertificate(string? status) =>
        Equals(LatestVeterinaryHealthCertificate, status);

    public static bool IsLetterOfAuthority(string? status) => Equals(LetterOfAuthority, status);

    public static bool IsLicenseOrAuthorisation(string? status) => Equals(LicenseOrAuthorisation, status);

    public static bool IsMycotoxinCertification(string? status) => Equals(MycotoxinCertification, status);

    public static bool IsOriginCertificate(string? status) => Equals(OriginCertificate, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    public static bool IsPhytosanitaryCertificate(string? status) => Equals(PhytosanitaryCertificate, status);

    public static bool IsProcessingStatement(string? status) => Equals(ProcessingStatement, status);

    public static bool IsProofOfStorage(string? status) => Equals(ProofOfStorage, status);

    public static bool IsRailwayBill(string? status) => Equals(RailwayBill, status);

    public static bool IsSeaWaybill(string? status) => Equals(SeaWaybill, status);

    public static bool IsVeterinaryHealthCertificate(string? status) => Equals(VeterinaryHealthCertificate, status);

    public static bool IsListOfIngredients(string? status) => Equals(ListOfIngredients, status);

    public static bool IsPackingList(string? status) => Equals(PackingList, status);

    public static bool IsRoadConsignmentNote(string? status) => Equals(RoadConsignmentNote, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

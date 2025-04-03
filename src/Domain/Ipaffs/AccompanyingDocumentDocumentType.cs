using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccompanyingDocumentDocumentType
{
    AirWaybill,

    BillOfLading,

    CargoManifest,

    CatchCertificate,

    CommercialDocument,

    CommercialInvoice,

    ConformityCertificate,

    ContainerManifest,

    CustomsDeclaration,

    Docom,

    HealthCertificate,

    HeatTreatmentCertificate,

    ImportPermit,

    InspectionCertificate,

    Itahc,

    JourneyLog,

    LaboratorySamplingResultsForAflatoxin,

    LatestVeterinaryHealthCertificate,

    LetterOfAuthority,

    LicenseOrAuthorisation,

    MycotoxinCertification,

    OriginCertificate,

    Other,

    PhytosanitaryCertificate,

    ProcessingStatement,

    ProofOfStorage,

    RailwayBill,

    SeaWaybill,

    VeterinaryHealthCertificate,

    ListOfIngredients,

    PackingList,

    RoadConsignmentNote,
}

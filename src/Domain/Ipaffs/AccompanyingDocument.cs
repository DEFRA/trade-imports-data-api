using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Accompanying document
/// </summary>
public class AccompanyingDocument
{
    /// <summary>
    /// Additional document type
    /// </summary>
    [JsonPropertyName("documentType")]
    [System.ComponentModel.Description("Additional document type")]
    [PossibleValue("airWaybill")]
    [PossibleValue("billOfLading")]
    [PossibleValue("cargoManifest")]
    [PossibleValue("catchCertificate")]
    [PossibleValue("commercialDocument")]
    [PossibleValue("commercialInvoice")]
    [PossibleValue("conformityCertificate")]
    [PossibleValue("containerManifest")]
    [PossibleValue("customsDeclaration")]
    [PossibleValue("docom")]
    [PossibleValue("healthCertificate")]
    [PossibleValue("heatTreatmentCertificate")]
    [PossibleValue("importPermit")]
    [PossibleValue("inspectionCertificate")]
    [PossibleValue("itahc")]
    [PossibleValue("journeyLog")]
    [PossibleValue("laboratorySamplingResultsForAflatoxin")]
    [PossibleValue("latestVeterinaryHealthCertificate")]
    [PossibleValue("letterOfAuthority")]
    [PossibleValue("licenseOrAuthorisation")]
    [PossibleValue("mycotoxinCertification")]
    [PossibleValue("originCertificate")]
    [PossibleValue("other")]
    [PossibleValue("phytosanitaryCertificate")]
    [PossibleValue("processingStatement")]
    [PossibleValue("proofOfStorage")]
    [PossibleValue("railwayBill")]
    [PossibleValue("seaWaybill")]
    [PossibleValue("veterinaryHealthCertificate")]
    [PossibleValue("listOfIngredients")]
    [PossibleValue("packingList")]
    [PossibleValue("roadConsignmentNote")]
    public string? DocumentType { get; set; }

    /// <summary>
    /// Additional document reference
    /// </summary>
    [JsonPropertyName("documentReference")]
    [System.ComponentModel.Description("Additional document reference")]
    public string? DocumentReference { get; set; }

    /// <summary>
    /// Additional document issue date
    /// </summary>
    [JsonPropertyName("documentIssuedOn")]
    [System.ComponentModel.Description("Additional document issue date")]
    public DateOnly? DocumentIssuedOn { get; set; }

    /// <summary>
    /// The UUID used for the uploaded file in blob storage
    /// </summary>
    [JsonPropertyName("attachmentId")]
    [System.ComponentModel.Description("The UUID used for the uploaded file in blob storage")]
    public string? AttachmentId { get; set; }

    /// <summary>
    /// The original filename of the uploaded file
    /// </summary>
    [JsonPropertyName("attachmentFilename")]
    [System.ComponentModel.Description("The original filename of the uploaded file")]
    public string? AttachmentFilename { get; set; }

    /// <summary>
    /// The MIME type of the uploaded file
    /// </summary>
    [JsonPropertyName("attachmentContentType")]
    [System.ComponentModel.Description("The MIME type of the uploaded file")]
    public string? AttachmentContentType { get; set; }

    /// <summary>
    /// The UUID for the user that uploaded the file
    /// </summary>
    [JsonPropertyName("uploadUserId")]
    [System.ComponentModel.Description("The UUID for the user that uploaded the file")]
    public string? UploadUserId { get; set; }

    /// <summary>
    /// The UUID for the organisation that the upload user is associated with
    /// </summary>
    [JsonPropertyName("uploadOrganisationId")]
    [System.ComponentModel.Description("The UUID for the organisation that the upload user is associated with")]
    public string? UploadOrganisationId { get; set; }

    /// <summary>
    /// External reference of accompanying document, which relates to a downstream service
    /// </summary>
    [JsonPropertyName("externalReference")]
    [System.ComponentModel.Description(
        "External reference of accompanying document, which relates to a downstream service"
    )]
    public ExternalReference? ExternalReference { get; set; }
}

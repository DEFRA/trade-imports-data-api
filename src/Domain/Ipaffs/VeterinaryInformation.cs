using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Part 1 - Holds the information related to veterinary checks and details
/// </summary>
public class VeterinaryInformation
{
    /// <summary>
    /// External reference of approved establishments, which relates to a downstream service
    /// </summary>

    [JsonPropertyName("establishmentsOfOriginExternalReference")]
    [System.ComponentModel.Description(
        "External reference of approved establishments, which relates to a downstream service"
    )]
    public ExternalReference? EstablishmentsOfOriginExternalReference { get; set; }

    /// <summary>
    /// List of establishments which were approved by UK to issue veterinary documents
    /// </summary>

    [JsonPropertyName("establishmentsOfOrigins")]
    [System.ComponentModel.Description(
        "List of establishments which were approved by UK to issue veterinary documents"
    )]
    public ApprovedEstablishment[]? EstablishmentsOfOrigins { get; set; }

    /// <summary>
    /// Veterinary document identification
    /// </summary>

    [JsonPropertyName("veterinaryDocument")]
    [System.ComponentModel.Description("Veterinary document identification")]
    public string? VeterinaryDocument { get; set; }

    /// <summary>
    /// Veterinary document issue date
    /// </summary>

    [JsonPropertyName("veterinaryDocumentIssuedOn")]
    [System.ComponentModel.Description("Veterinary document issue date")]
    public DateOnly? VeterinaryDocumentIssuedOn { get; set; }

    /// <summary>
    /// Additional documents
    /// </summary>

    [JsonPropertyName("accompanyingDocumentNumbers")]
    [System.ComponentModel.Description("Additional documents")]
    public string[]? AccompanyingDocumentNumbers { get; set; }

    /// <summary>
    /// Accompanying documents
    /// </summary>

    [JsonPropertyName("accompanyingDocuments")]
    [System.ComponentModel.Description("Accompanying documents")]
    public AccompanyingDocument[]? AccompanyingDocuments { get; set; }

    /// <summary>
    /// Catch certificate attachments
    /// </summary>

    [JsonPropertyName("catchCertificateAttachments")]
    [System.ComponentModel.Description("Catch certificate attachments")]
    public CatchCertificateAttachment[]? CatchCertificateAttachments { get; set; }

    /// <summary>
    /// Details helpful for identification
    /// </summary>

    [JsonPropertyName("identificationDetails")]
    [System.ComponentModel.Description("Details helpful for identification")]
    public IdentificationDetails[]? IdentificationDetails { get; set; }
}

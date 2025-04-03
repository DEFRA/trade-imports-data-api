using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Catch certificate attachment
/// </summary>
public class CatchCertificateAttachment
{
    /// <summary>
    /// The UUID of the uploaded catch certificate file in blob storage
    /// </summary>

    [JsonPropertyName("attachmentId")]
    [System.ComponentModel.Description("The UUID of the uploaded catch certificate file in blob storage")]
    public string? AttachmentId { get; set; }

    /// <summary>
    /// The total number of catch certificates on the attachment
    /// </summary>

    [JsonPropertyName("numberOfCatchCertificates")]
    [System.ComponentModel.Description("The total number of catch certificates on the attachment")]
    public int? NumberOfCatchCertificates { get; set; }

    /// <summary>
    /// List of catch certificate details
    /// </summary>

    [JsonPropertyName("catchCertificateDetails")]
    [System.ComponentModel.Description("List of catch certificate details")]
    public CatchCertificateDetails[]? CatchCertificateDetails { get; set; }
}

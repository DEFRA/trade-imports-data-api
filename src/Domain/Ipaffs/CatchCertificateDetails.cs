using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Catch certificate details for uploaded attachment
/// </summary>
public class CatchCertificateDetails
{
    /// <summary>
    /// The UUID of the catch certificate
    /// </summary>

    [JsonPropertyName("catchCertificateId")]
    [System.ComponentModel.Description("The UUID of the catch certificate")]
    public string? CatchCertificateId { get; set; }

    /// <summary>
    /// Catch certificate reference
    /// </summary>

    [JsonPropertyName("catchCertificateReference")]
    [System.ComponentModel.Description("Catch certificate reference")]
    public string? CatchCertificateReference { get; set; }

    /// <summary>
    /// Catch certificate date of issue
    /// </summary>

    [JsonPropertyName("issuedOn")]
    [System.ComponentModel.Description("Catch certificate date of issue")]
    public DateOnly? IssuedOn { get; set; }

    /// <summary>
    /// Catch certificate flag state of catching vessel(s)
    /// </summary>

    [JsonPropertyName("flagState")]
    [System.ComponentModel.Description("Catch certificate flag state of catching vessel(s)")]
    public string? FlagState { get; set; }

    /// <summary>
    /// List of species imported under this catch certificate
    /// </summary>

    [JsonPropertyName("species")]
    [System.ComponentModel.Description("List of species imported under this catch certificate")]
    public string[]? Species { get; set; }
}

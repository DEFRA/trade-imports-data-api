using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

public class CatchCertificates
{
    /// <summary>
    /// The catch certificate number
    /// </summary>

    [JsonPropertyName("certificateNumber")]
    [System.ComponentModel.Description("The catch certificate number")]
    public string? CertificateNumber { get; set; }

    /// <summary>
    /// The catch certificate weight number
    /// </summary>

    [JsonPropertyName("weight")]
    [System.ComponentModel.Description("The catch certificate weight number")]
    public double? Weight { get; set; }
}

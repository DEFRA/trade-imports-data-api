using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

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

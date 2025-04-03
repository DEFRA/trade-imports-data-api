using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// PHSI Decision Breakdown
/// </summary>
public class Phsi
{
    /// <summary>
    /// Whether or not a documentary check is required for PHSI
    /// </summary>

    [JsonPropertyName("documentCheck")]
    [System.ComponentModel.Description("Whether or not a documentary check is required for PHSI")]
    public bool? DocumentCheck { get; set; }

    /// <summary>
    /// Whether or not an identity check is required for PHSI
    /// </summary>

    [JsonPropertyName("identityCheck")]
    [System.ComponentModel.Description("Whether or not an identity check is required for PHSI")]
    public bool? IdentityCheck { get; set; }

    /// <summary>
    /// Whether or not a physical check is required for PHSI
    /// </summary>

    [JsonPropertyName("physicalCheck")]
    [System.ComponentModel.Description("Whether or not a physical check is required for PHSI")]
    public bool? PhysicalCheck { get; set; }
}

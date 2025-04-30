using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Information about not acceptable reason
/// </summary>
public class ChedppNotAcceptableReason
{
    /// <summary>
    /// reason for refusal
    /// </summary>
    [JsonPropertyName("reason")]
    [System.ComponentModel.Description("reason for refusal")]
    public string? Reason { get; set; }

    /// <summary>
    /// commodity or package
    /// </summary>
    [JsonPropertyName("commodityOrPackage")]
    [System.ComponentModel.Description("commodity or package")]
    public string? CommodityOrPackage { get; set; }
}

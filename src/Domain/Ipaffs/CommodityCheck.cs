using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class CommodityCheck
{
    /// <summary>
    /// UUID used to match the commodityChecks to the commodityComplement
    /// </summary>
    [JsonPropertyName("uniqueComplementId")]
    public string? UniqueComplementId { get; set; }

    [JsonPropertyName("checks")]
    public InspectionCheck[]? Checks { get; set; }

    /// <summary>
    /// Manually entered validity period, allowed if risk decision is INSPECTION_REQUIRED and HMI check status 'Compliant' or 'Not inspected'
    /// </summary>
    [JsonPropertyName("validityPeriod")]
    public int ValidityPeriod { get; set; }
}

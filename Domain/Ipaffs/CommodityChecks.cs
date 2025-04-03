using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

public class CommodityChecks
{
    /// <summary>
    /// UUID used to match the commodityChecks to the commodityComplement
    /// </summary>

    [JsonPropertyName("uniqueComplementId")]
    [System.ComponentModel.Description("UUID used to match the commodityChecks to the commodityComplement")]
    public string? UniqueComplementId { get; set; }

    [JsonPropertyName("checks")]
    public InspectionCheck[]? Checks { get; set; }

    /// <summary>
    /// Manually entered validity period, allowed if risk decision is INSPECTION_REQUIRED and HMI check status &#x27;Compliant&#x27; or &#x27;Not inspected&#x27;
    /// </summary>

    [JsonPropertyName("validityPeriod")]
    [System.ComponentModel.Description(
        "Manually entered validity period, allowed if risk decision is INSPECTION_REQUIRED and HMI check status 'Compliant' or 'Not inspected'"
    )]
    public int? ValidityPeriod { get; set; }
}

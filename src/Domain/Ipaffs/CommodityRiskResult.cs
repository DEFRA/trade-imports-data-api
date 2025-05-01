using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Result of the risk assessment of a commodity
/// </summary>
public class CommodityRiskResult
{
    /// <summary>
    /// CHED-A, CHED-D, CHED-P - what is the commodity complement risk decision
    /// </summary>
    [JsonPropertyName("riskDecision")]
    [System.ComponentModel.Description("CHED-A, CHED-D, CHED-P - what is the commodity complement risk decision")]
    [PossibleValue("REQUIRED")]
    [PossibleValue("NOTREQUIRED")]
    [PossibleValue("INCONCLUSIVE")]
    [PossibleValue("REENFORCED_CHECK")]
    public string? RiskDecision { get; set; }

    /// <summary>
    /// Transit CHED - what is the commodity complement exit risk decision
    /// </summary>
    [JsonPropertyName("exitRiskDecision")]
    [System.ComponentModel.Description("Transit CHED - what is the commodity complement exit risk decision")]
    [PossibleValue("REQUIRED")]
    [PossibleValue("NOTREQUIRED")]
    [PossibleValue("INCONCLUSIVE")]
    public string? ExitRiskDecision { get; set; }

    /// <summary>
    /// HMI decision required
    /// </summary>
    [JsonPropertyName("hmiDecision")]
    [System.ComponentModel.Description("HMI decision required")]
    [PossibleValue("REQUIRED")]
    [PossibleValue("NOTREQUIRED")]
    public string? HmiDecision { get; set; }

    /// <summary>
    /// PHSI decision required
    /// </summary>
    [JsonPropertyName("phsiDecision")]
    [System.ComponentModel.Description("PHSI decision required")]
    [PossibleValue("REQUIRED")]
    [PossibleValue("NOTREQUIRED")]
    public string? PhsiDecision { get; set; }

    /// <summary>
    /// PHSI classification
    /// </summary>
    [JsonPropertyName("phsiClassification")]
    [System.ComponentModel.Description("PHSI classification")]
    [PossibleValue("Mandatory")]
    [PossibleValue("Reduced")]
    [PossibleValue("Controlled")]
    public string? PhsiClassification { get; set; }

    /// <summary>
    /// PHSI Decision Breakdown
    /// </summary>
    [JsonPropertyName("phsi")]
    [System.ComponentModel.Description("PHSI Decision Breakdown")]
    public Phsi? Phsi { get; set; }

    /// <summary>
    /// UUID used to match to the complement parameter set
    /// </summary>
    [JsonPropertyName("uniqueId")]
    [System.ComponentModel.Description("UUID used to match to the complement parameter set")]
    public string? UniqueId { get; set; }

    /// <summary>
    /// EPPO Code for the species
    /// </summary>
    [JsonPropertyName("eppoCode")]
    [System.ComponentModel.Description("EPPO Code for the species")]
    public string? EppoCode { get; set; }

    /// <summary>
    /// Name or ID of the variety
    /// </summary>
    [JsonPropertyName("variety")]
    [System.ComponentModel.Description("Name or ID of the variety")]
    public string? Variety { get; set; }

    /// <summary>
    /// Whether or not a plant is woody
    /// </summary>
    [JsonPropertyName("isWoody")]
    [System.ComponentModel.Description("Whether or not a plant is woody")]
    public bool? IsWoody { get; set; }

    /// <summary>
    /// Indoor or Outdoor for a plant
    /// </summary>
    [JsonPropertyName("indoorOutdoor")]
    [System.ComponentModel.Description("Indoor or Outdoor for a plant")]
    public string? IndoorOutdoor { get; set; }

    /// <summary>
    /// Whether the propagation is considered a Plant, Bulb, Seed or None
    /// </summary>
    [JsonPropertyName("propagation")]
    [System.ComponentModel.Description("Whether the propagation is considered a Plant, Bulb, Seed or None")]
    public string? Propagation { get; set; }

    /// <summary>
    /// Rule type for PHSI checks
    /// </summary>
    [JsonPropertyName("phsiRuleType")]
    [System.ComponentModel.Description("Rule type for PHSI checks")]
    public string? PhsiRuleType { get; set; }
}

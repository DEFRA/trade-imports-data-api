using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;
using Defra.TradeImportsDataApi.Domain.Json;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Details of the risk categorisation level for a notification
/// </summary>
public class JourneyRiskCategorisationResult
{
    /// <summary>
    /// Risk Level is defined using enum values High,Medium,Low
    /// </summary>
    [JsonPropertyName("riskLevel")]
    [System.ComponentModel.Description("Risk Level is defined using enum values High,Medium,Low")]
    [PossibleValue("High")]
    [PossibleValue("Medium")]
    [PossibleValue("Low")]
    public string? RiskLevel { get; set; }

    /// <summary>
    /// Indicator of whether the risk level was determined by the system or by the user
    /// </summary>
    [JsonPropertyName("riskLevelMethod")]
    [System.ComponentModel.Description(
        "Indicator of whether the risk level was determined by the system or by the user"
    )]
    [PossibleValue("System")]
    [PossibleValue("User")]
    public string? RiskLevelMethod { get; set; }

    /// <summary>
    /// The date and time the risk level has been set for a notification
    /// </summary>
    [JsonPropertyName("riskLevelSetFor")]
    [System.ComponentModel.Description("The date and time the risk level has been set for a notification")]
    [UnknownTimeZoneDateTimeJsonConverter(nameof(RiskLevelSetFor))]
    public DateTime? RiskLevelSetFor { get; set; }
}

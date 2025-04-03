using System.Text.Json.Serialization;
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
    public JourneyRiskCategorisationResultRiskLevel? RiskLevel { get; set; }

    /// <summary>
    /// Indicator of whether the risk level was determined by the system or by the user
    /// </summary>

    [JsonPropertyName("riskLevelMethod")]
    [System.ComponentModel.Description(
        "Indicator of whether the risk level was determined by the system or by the user"
    )]
    public JourneyRiskCategorisationResultRiskLevelMethod? RiskLevelMethod { get; set; }

    /// <summary>
    /// The date and time the risk level has been set for a notification
    /// </summary>

    [JsonPropertyName("riskLevelSetFor")]
    [System.ComponentModel.Description("The date and time the risk level has been set for a notification")]
    [
        UnknownTimeZoneDateTimeJsonConverter(nameof(RiskLevelSetFor)),
        MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)
    ]
    public DateTime? RiskLevelSetFor { get; set; }
}

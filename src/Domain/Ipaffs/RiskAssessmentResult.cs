using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Json;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Result of risk assessment by the risk scorer
/// </summary>
public class RiskAssessmentResult
{
    /// <summary>
    /// List of risk assessed commodities
    /// </summary>

    [JsonPropertyName("commodityResults")]
    [System.ComponentModel.Description("List of risk assessed commodities")]
    public CommodityRiskResult[]? CommodityResults { get; set; }

    /// <summary>
    /// Date and time of assessment
    /// </summary>

    [JsonPropertyName("assessedOn")]
    [System.ComponentModel.Description("Date and time of assessment")]
    [
        UnknownTimeZoneDateTimeJsonConverter(nameof(AssessedOn)),
        MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)
    ]
    public DateTime? AssessedOn { get; set; }
}

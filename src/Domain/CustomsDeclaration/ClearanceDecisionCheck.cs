using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ClearanceDecisionCheck
{
    [JsonPropertyName("checkCode")]
    public required string CheckCode { get; set; }

    [JsonPropertyName("decisionCode")]
    public required string DecisionCode { get; set; }

    [JsonPropertyName("decisionsValidUntil")]
    public DateTime? DecisionsValidUntil { get; set; }

    [JsonPropertyName("decisionReasons")]
    public string[]? DecisionReasons { get; set; }

    [JsonPropertyName("decisionInternalFurtherDetail")]
    public string[]? DecisionInternalFurtherDetail { get; set; }
}

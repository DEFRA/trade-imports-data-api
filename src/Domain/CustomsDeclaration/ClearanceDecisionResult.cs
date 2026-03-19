using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ClearanceDecisionResult
{
    [JsonPropertyName("itemNumber")]
    public int ItemNumber { get; set; }

    [JsonPropertyName("importPreNotification")]
    public string? ImportPreNotification { get; set; }

    [JsonPropertyName("documentReference")]
    public string? DocumentReference { get; set; }

    [JsonPropertyName("documentCode")]
    public string? DocumentCode { get; set; }

    [JsonPropertyName("checkCode")]
    public string? CheckCode { get; set; }

    [JsonPropertyName("decisionCode")]
    public string? DecisionCode { get; set; }

    [JsonPropertyName("decisionReason")]
    public string? DecisionReason { get; set; }

    [JsonPropertyName("internalDecisionCode")]
    public string? InternalDecisionCode { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("ruleName")]
    public string? RuleName { get; set; }
}

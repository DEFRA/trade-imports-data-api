using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ClearanceDecision
{
    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("externalVersionNumber")]
    public int? ExternalVersionNumber { get; set; }

    [JsonPropertyName("decisionNumber")]
    public int? DecisionNumber { get; set; }

    [JsonPropertyName("sourceVersion")]
    public string? SourceVersion { get; set; }

    [JsonPropertyName("items")]
    public required ClearanceDecisionItem[] Items { get; set; } = [];

    [JsonPropertyName("results")]
    public ClearanceDecisionResult[]? Results { get; set; } = [];
}

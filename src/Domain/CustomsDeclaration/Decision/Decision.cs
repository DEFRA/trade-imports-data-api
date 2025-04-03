using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration.Decision;

public class Decision
{
    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("entryReference")]
    public string? EntryReference { get; set; }

    [JsonPropertyName("entryVersionNumber")]
    public int? EntryVersionNumber { get; set; }

    [JsonPropertyName("decisionNumber")]
    public int? DecisionNumber { get; set; }

    [JsonPropertyName("items")]
    public required DecisionItem[] Items { get; set; }
}

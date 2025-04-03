using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration.Finalisation;

public class Finalisation
{
    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("entryReference")]
    public required string EntryReference { get; set; }

    [JsonPropertyName("entryVersionNumber")]
    public required int EntryVersionNumber { get; set; }

    [JsonPropertyName("decisionNumber")]
    public int? DecisionNumber { get; set; }

    [JsonPropertyName("finalState")]
    public required FinalState FinalState { get; set; }

    [JsonPropertyName("manualAction")]
    public required bool ManualAction { get; set; }
}

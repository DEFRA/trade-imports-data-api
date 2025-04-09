using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class Finalisation
{
    [JsonPropertyName("externalCorrelationId")]
    public string? ExternalCorrelationId { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("externalVersion")]
    public required int ExternalVersion { get; set; }

    [JsonPropertyName("decisionNumber")]
    public int? DecisionNumber { get; set; }

    [JsonPropertyName("finalState")]
    public required FinalState FinalState { get; set; }

    [JsonPropertyName("isManualRelease")]
    public required bool IsManualRelease { get; set; }
}

using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class Finalisation
{
    [JsonPropertyName("externalCorrelationId")]
    public string? ExternalCorrelationId { get; set; }

    [JsonPropertyName("messageSentAt")]
    public DateTime MessageSentAt { get; set; }

    [JsonPropertyName("externalVersion")]
    public required int ExternalVersion { get; set; }

    [JsonPropertyName("decisionNumber")]
    public int? DecisionNumber { get; set; }

    [PossibleValue("0")]
    [PossibleValue("1")]
    [PossibleValue("2")]
    [PossibleValue("3")]
    [PossibleValue("4")]
    [PossibleValue("5")]
    [PossibleValue("6")]
    [JsonPropertyName("finalState")]
    public required string FinalState { get; set; }

    [JsonPropertyName("isManualRelease")]
    public required bool IsManualRelease { get; set; }
}

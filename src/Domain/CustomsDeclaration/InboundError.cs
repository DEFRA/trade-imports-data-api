using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class InboundError
{
    [JsonPropertyName("externalCorrelationId")]
    public string? ExternalCorrelationId { get; set; }

    [JsonPropertyName("externalVersion")]
    public int? ExternalVersion { get; set; }

    [JsonPropertyName("errors")]
    public InboundErrorItem[]? Errors { get; set; }
}

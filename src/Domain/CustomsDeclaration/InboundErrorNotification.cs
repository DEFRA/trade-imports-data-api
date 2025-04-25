using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class InboundErrorNotification
{
    [JsonPropertyName("externalCorrelationId")]
    public string? ExternalCorrelationId { get; set; }

    [JsonPropertyName("externalVersion")]
    public int? ExternalVersion { get; set; }

    [JsonPropertyName("errors")]
    public InboundErrorItem[]? Errors { get; set; }
}

using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Errors;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ExternalError
{
    [JsonPropertyName("externalCorrelationId")]
    public string? ExternalCorrelationId { get; set; }

    [JsonPropertyName("sourceCorrelationId")]
    public string? SourceCorrelationId { get; set; }

    [JsonPropertyName("externalVersion")]
    public int? ExternalVersion { get; set; }

    [JsonPropertyName("messageSentAt")]
    public DateTime MessageSentAt { get; set; }

    [JsonPropertyName("errors")]
    public ErrorItem[]? Errors { get; set; }
}

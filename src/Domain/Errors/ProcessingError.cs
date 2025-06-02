using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Errors;

public class ProcessingError
{
    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("sourceExternalCorrelationId")]
    public string? SourceExternalCorrelationId { get; set; }

    [JsonPropertyName("externalVersion")]
    public int? ExternalVersion { get; set; }

    [JsonPropertyName("created")]
    public DateTime? Created { get; set; }

    [JsonPropertyName("errors")]
    public ErrorItem[] Errors { get; set; } = [];

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

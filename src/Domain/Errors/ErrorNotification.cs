using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Errors;

public class ErrorNotification
{
    [JsonPropertyName("externalCorrelationId")]
    public string? ExternalCorrelationId { get; set; }

    [JsonPropertyName("externalVersion")]
    public int? ExternalVersion { get; set; }

    [JsonPropertyName("errors")]
    public ErrorItem[] Errors { get; set; } = [];

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

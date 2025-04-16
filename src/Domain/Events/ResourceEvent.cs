using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Events;

public record ResourceEvent<T>
{
    [JsonPropertyName("resourceId")]
    public required string ResourceId { get; init; }

    [JsonPropertyName("resourceType")]
    public required string ResourceType { get; init; }

    [JsonPropertyName("operation")]
    public required string Operation { get; init; }

    [JsonPropertyName("resource")]
    public T? Resource { get; init; }

    [JsonPropertyName("etag")]
    public string? ETag { get; init; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    [JsonPropertyName("changeSet")]
    public List<Diff> ChangeSet { get; init; } = [];
}

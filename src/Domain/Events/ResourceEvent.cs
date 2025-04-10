using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class ResourceEvent<T>
{
    [JsonPropertyName("resourceId")]
    public required string ResourceId { get; set; }

    [JsonPropertyName("resourceType")]
    public required string ResourceType { get; set; }

    [JsonPropertyName("operation")]
    public required string Operation { get; set; }

    [JsonPropertyName("resource")]
    public required T Resource { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("changeSet")]
    public List<Diff> ChangeSet { get; set; } = null!;
}

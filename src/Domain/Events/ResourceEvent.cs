using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class ResourceEvent<T>
{
    [JsonPropertyName("entityId")]
    public required string EntityId { get; set; }

    [JsonPropertyName("entityType")]
    public required string EntityType { get; set; }

    [JsonPropertyName("operation")]
    public required string Operation { get; set; }

    [JsonPropertyName("body")]
    public required T Body { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("changeSet")]
    public List<Diff> ChangeSet { get; set; } = null!;
}
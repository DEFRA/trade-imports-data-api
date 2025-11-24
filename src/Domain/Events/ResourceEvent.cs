using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Events;

public record ResourceEvent<T>
{
    public required string ResourceId { get; init; }

    public required string ResourceType { get; init; }

    public string? SubResourceType { get; init; }

    public required string Operation { get; init; }

    public T? Resource { get; init; }

    public string? Etag { get; init; }

    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public List<Diff> ChangeSet { get; init; } = [];
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Events;

/// <summary>
/// This event is raised when a resource is created, updated or deleted.
/// </summary>
/// <typeparam name="T"></typeparam>
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

using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Domain.Events;

/// <summary>
/// This event is raised to record an activity against a resource.
/// </summary>
/// <typeparam name="T"></typeparam>
[ExcludeFromCodeCoverage]
public record BtmsActivityEvent<T>
{
    public required string OriginatingServiceName { get; init; }

    public required string ResourceId { get; init; }

    public required string ResourceType { get; init; }

    public string? SubResourceType { get; init; }

    public required T Activity { get; init; }

    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

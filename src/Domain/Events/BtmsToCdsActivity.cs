using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Domain.Events;

[ExcludeFromCodeCoverage]
public record BtmsToCdsActivity
{
    public required string CorrelationId { get; init; }

    public int StatusCode { get; init; }

    public DateTime Timestamp { get; init; }
}

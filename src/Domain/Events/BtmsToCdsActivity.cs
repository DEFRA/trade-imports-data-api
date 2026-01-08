using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Domain.Events;

[ExcludeFromCodeCoverage]
public record BtmsToCdsActivity
{
    public required string CorrelationId { get; init; }

    public required int ResponseStatusCode { get; init; }

    public required DateTime ResponseTimestamp { get; init; }
}

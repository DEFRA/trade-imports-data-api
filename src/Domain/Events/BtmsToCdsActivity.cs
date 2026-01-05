namespace Defra.TradeImportsDataApi.Domain.Events;

public record BtmsToCdsActivity
{
    public required string CorrelationId { get; init; }

    public int StatusCode { get; init; }

    public DateTime Timestamp { get; init; }
}

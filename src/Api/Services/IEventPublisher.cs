namespace Defra.TradeImportsDataApi.Api.Services;

public interface IEventPublisher
{
    Task Publish<T>(T @event, CancellationToken cancellationToken);
}

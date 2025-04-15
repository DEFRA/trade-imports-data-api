namespace Defra.TradeImportsDataApi.Api.Services;

public interface IEventPublisher
{
    Task Publish<T>(T @event, string resourceType, CancellationToken cancellationToken);
}

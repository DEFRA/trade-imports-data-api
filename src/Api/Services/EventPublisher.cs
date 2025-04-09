namespace Defra.TradeImportsDataApi.Api.Services;

public class EventPublisher : IEventPublisher
{
    public Task Publish<T>(T @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
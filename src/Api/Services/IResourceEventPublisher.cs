using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IResourceEventPublisher
{
    Task Publish<T>(ResourceEvent<T> @event, CancellationToken cancellationToken);
}

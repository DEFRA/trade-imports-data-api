using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IResourceEventPublisher
{
    Task Publish<T>(ResourceEvent<T> @event, CancellationToken cancellationToken);
}

public interface IResourceEventEntityPublisher
{
    Task Publish(ResourceEventEntity entity, CancellationToken cancellationToken);
}

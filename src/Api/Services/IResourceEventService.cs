using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IResourceEventService
{
    Task Publish(ResourceEventEntity entity, CancellationToken cancellationToken);

    Task PublishAllowException(ResourceEventEntity entity, CancellationToken cancellationToken);
}

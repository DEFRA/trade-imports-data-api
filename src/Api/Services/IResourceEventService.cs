using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IResourceEventService
{
    Task<ResourceEventEntity> Publish(ResourceEventEntity entity, CancellationToken cancellationToken);

    Task<ResourceEventEntity> PublishAllowException(ResourceEventEntity entity, CancellationToken cancellationToken);
}

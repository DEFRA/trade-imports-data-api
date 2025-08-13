using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IResourceEventPublisher
{
    Task Publish(ResourceEventEntity entity, CancellationToken cancellationToken);
}

using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IResourceEventRepository
{
    ResourceEventEntity Insert<T>(ResourceEvent<T> @event);

    Task<List<ResourceEventEntity>> GetAll(string resourceId, CancellationToken cancellationToken);
}

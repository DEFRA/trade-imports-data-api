using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using Defra.TradeImportsDataApi.Domain.Events;
using MongoDB.Bson;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ResourceEventRepository(IDbContext dbContext) : IResourceEventRepository
{
    public ResourceEventEntity Insert<T>(ResourceEvent<T> @event)
    {
        // Needs to set TTL

        var entity = new ResourceEventEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            ResourceId = @event.ResourceId,
            ResourceType = @event.ResourceType,
            ResourceEvent = @event,
        };

        dbContext.ResourceEvents.Insert(entity);

        return entity;
    }

    public async Task<List<ResourceEventEntity>> GetAll(string resourceId, CancellationToken cancellationToken) =>
        await dbContext
            .ResourceEvents.Where(x => x.ResourceId == resourceId)
            .ToListWithFallbackAsync(cancellationToken);
}

using System.Text.Json;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using Defra.TradeImportsDataApi.Domain.Events;
using MongoDB.Bson;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ResourceEventRepository(IDbContext dbContext) : IResourceEventRepository
{
    private static readonly TimeSpan s_defaultTtl = TimeSpan.FromDays(30);

    public ResourceEventEntity Insert<T>(ResourceEvent<T> @event)
    {
        var entity = new ResourceEventEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            ResourceId = @event.ResourceId,
            ResourceType = @event.ResourceType,
            SubResourceType = @event.SubResourceType,
            Operation = @event.Operation,
            Message = JsonSerializer.Serialize(@event),
            ExpiresAt = DateTime.UtcNow.Add(s_defaultTtl), // See index for where TTL if enforced
        };

        dbContext.ResourceEvents.Insert(entity);

        return entity;
    }

    public ResourceEventEntity Update(ResourceEventEntity entity)
    {
        dbContext.ResourceEvents.Update(entity, entity.ETag);

        return entity;
    }

    public async Task<List<ResourceEventEntity>> GetAll(string resourceId, CancellationToken cancellationToken) =>
        await dbContext
            .ResourceEvents.Where(x => x.ResourceId == resourceId)
            .ToListWithFallbackAsync(cancellationToken);
}

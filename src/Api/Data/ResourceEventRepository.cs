using System.Text.Json;
using Defra.TradeImportsDataApi.Api.Configuration;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using Defra.TradeImportsDataApi.Domain.Events;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ResourceEventRepository(IDbContext dbContext, IOptions<ResourceEventOptions> options)
    : IResourceEventRepository
{
    public ResourceEventEntity Insert<T>(ResourceEvent<T> @event)
    {
        var entity = new ResourceEventEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            ResourceId = @event.ResourceId,
            ResourceType = @event.ResourceType,
            SubResourceType = @event.SubResourceType,
            Operation = @event.Operation,
            Message = JsonSerializer.Serialize(@event, JsonSettings.Instance),
            ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromDays(options.Value.TtlDays)), // See index for where TTL if enforced
        };

        dbContext.ResourceEvents.Insert(entity);

        return entity;
    }

    public ResourceEventEntity UpdateProcessed(ResourceEventEntity entity)
    {
        dbContext.ResourceEvents.Update(
            entity,
            x =>
            {
                // Update in memory item now and set field to match value,
                // but will only be saved if Save is called
                entity.Published = DateTime.UtcNow;
                x.Set(y => y.Published, entity.Published);
            },
            entity.ETag
        );

        return entity;
    }

    public async Task<List<ResourceEventEntity>> GetAll(string resourceId, CancellationToken cancellationToken) =>
        await dbContext
            .ResourceEvents.Where(x => x.ResourceId == resourceId)
            .ToListWithFallbackAsync(cancellationToken);
}

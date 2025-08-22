using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ResourceEventService(
    IDbContext dbContext,
    IResourceEventRepository resourceEventRepository,
    IResourceEventPublisher resourceEventPublisher,
    ILogger<ResourceEventService> logger
) : IResourceEventService
{
    public async Task<ResourceEventEntity> Publish(ResourceEventEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            return await PublishInternal(entity, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to publish resource event");

            // Intentionally swallowed
        }

        return entity;
    }

    public async Task<ResourceEventEntity> PublishAllowException(
        ResourceEventEntity entity,
        CancellationToken cancellationToken
    ) => await PublishInternal(entity, cancellationToken);

    private async Task<ResourceEventEntity> PublishInternal(
        ResourceEventEntity entity,
        CancellationToken cancellationToken
    )
    {
        await resourceEventPublisher.Publish(entity, cancellationToken);

        entity = resourceEventRepository.UpdateProcessed(entity);

        await dbContext.SaveChanges(cancellationToken);

        return entity;
    }
}

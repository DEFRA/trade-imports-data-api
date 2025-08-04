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
    public async Task Publish(ResourceEventEntity entity, CancellationToken cancellationToken)
    {
        try
        {
            await PublishInternal(entity, cancellationToken);
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
    }

    public async Task PublishAllowException(ResourceEventEntity entity, CancellationToken cancellationToken)
    {
        await PublishInternal(entity, cancellationToken);
    }

    private async Task PublishInternal(ResourceEventEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);

        await resourceEventPublisher.Publish(entity, cancellationToken);

        entity.Published = DateTime.UtcNow;

        resourceEventRepository.Update(entity);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);
    }
}

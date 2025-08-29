using System.Text.Json;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ResourceEventService(
    IDbContext dbContext,
    IResourceEventRepository resourceEventRepository,
    IResourceEventPublisher resourceEventPublisher,
    ILogger<ResourceEventService> logger,
    IReportRepository reportRepository
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

        await dbContext.StartTransaction(cancellationToken);

        entity = resourceEventRepository.UpdateProcessed(entity);

        Report(entity);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        return entity;
    }

    private void Report(ResourceEventEntity entity)
    {
        ReportClearanceRequest(entity);
        ReportClearanceDecision(entity);
        ReportFinalisation(entity);
        ReportImportPreNotification(entity);
    }

    private void ReportImportPreNotification(ResourceEventEntity entity)
    {
        if (entity is not { ResourceType: ResourceEventResourceTypes.ImportPreNotification })
            return;

        var @event =
            JsonSerializer.Deserialize<ResourceEvent<ImportPreNotificationEntity>>(entity.Message)
            ?? throw new InvalidOperationException("Could not deserialize event");

        reportRepository.ImportPreNotification(@event.Resource?.ImportPreNotification);
    }

    private void ReportFinalisation(ResourceEventEntity entity)
    {
        if (
            entity
            is not {
                ResourceType: ResourceEventResourceTypes.CustomsDeclaration,
                SubResourceType: ResourceEventSubResourceTypes.Finalisation
            }
        )
            return;

        var @event =
            JsonSerializer.Deserialize<ResourceEvent<CustomsDeclarationEntity>>(entity.Message)
            ?? throw new InvalidOperationException("Could not deserialize event");

        reportRepository.Finalisation(entity.ResourceId, @event.Resource?.Finalisation);
    }

    private void ReportClearanceDecision(ResourceEventEntity entity)
    {
        if (
            entity
            is not {
                ResourceType: ResourceEventResourceTypes.CustomsDeclaration,
                SubResourceType: ResourceEventSubResourceTypes.ClearanceDecision
            }
        )
            return;

        var @event =
            JsonSerializer.Deserialize<ResourceEvent<CustomsDeclarationEntity>>(entity.Message)
            ?? throw new InvalidOperationException("Could not deserialize event");

        reportRepository.ClearanceDecision(entity.ResourceId, @event.Resource?.ClearanceDecision);
    }

    private void ReportClearanceRequest(ResourceEventEntity entity)
    {
        if (
            entity
            is not {
                ResourceType: ResourceEventResourceTypes.CustomsDeclaration,
                SubResourceType: ResourceEventSubResourceTypes.ClearanceRequest
            }
        )
            return;

        reportRepository.ClearanceRequest(entity.ResourceId);
    }
}

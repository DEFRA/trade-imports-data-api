using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Services;

public class CustomsDeclarationService(
    IDbContext dbContext,
    IResourceEventPublisher resourceEventPublisher,
    ICustomsDeclarationRepository customsDeclarationRepository,
    IImportPreNotificationRepository importPreNotificationRepository
) : ICustomsDeclarationService
{
    public async Task<CustomsDeclarationEntity?> GetCustomsDeclaration(
        string id,
        CancellationToken cancellationToken
    ) => await customsDeclarationRepository.Get(id, cancellationToken);

    public async Task<CustomsDeclarationEntity> Insert(
        CustomsDeclarationEntity entity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var inserted = await customsDeclarationRepository.Insert(entity, cancellationToken);

        await TrackImportPreNotificationUpdate(inserted, cancellationToken);

        await dbContext.SaveChanges(cancellationToken);

        await resourceEventPublisher.Publish(
            inserted
                .ToResourceEvent(ResourceEventOperations.Created)
                .WithChangeSet(
                    new CustomsDeclaration
                    {
                        ClearanceRequest = inserted.ClearanceRequest,
                        ClearanceDecision = inserted.ClearanceDecision,
                        Finalisation = inserted.Finalisation,
                        ExternalErrors = inserted.ExternalErrors,
                    },
                    new CustomsDeclaration()
                ),
            cancellationToken
        );

        await dbContext.CommitTransaction(cancellationToken);

        return inserted;
    }

    public async Task<List<CustomsDeclarationEntity>> GetCustomsDeclarationsByChedId(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        var identifier = new ChedIdReference(chedId).GetIdentifier();

        return await customsDeclarationRepository.GetAll(identifier, cancellationToken);
    }

    public async Task<CustomsDeclarationEntity> Update(
        CustomsDeclarationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var (existing, updated) = await customsDeclarationRepository.Update(entity, etag, cancellationToken);

        await TrackImportPreNotificationUpdate(updated, cancellationToken);

        await dbContext.SaveChanges(cancellationToken);

        await resourceEventPublisher.Publish(
            updated
                .ToResourceEvent(ResourceEventOperations.Updated)
                .WithChangeSet(
                    new CustomsDeclaration
                    {
                        ClearanceRequest = updated.ClearanceRequest,
                        ClearanceDecision = updated.ClearanceDecision,
                        Finalisation = updated.Finalisation,
                        ExternalErrors = updated.ExternalErrors,
                    },
                    new CustomsDeclaration
                    {
                        ClearanceRequest = existing.ClearanceRequest,
                        ClearanceDecision = existing.ClearanceDecision,
                        Finalisation = existing.Finalisation,
                        ExternalErrors = existing.ExternalErrors,
                    }
                ),
            cancellationToken
        );

        await dbContext.CommitTransaction(cancellationToken);

        return updated;
    }

    private async Task TrackImportPreNotificationUpdate(
        CustomsDeclarationEntity entity,
        CancellationToken cancellationToken
    )
    {
        if (entity.ImportPreNotificationIdentifiers.Count <= 0)
            return;

        await importPreNotificationRepository.TrackImportPreNotificationUpdate(
            entity,
            entity.ImportPreNotificationIdentifiers.ToArray(),
            cancellationToken
        );
    }
}

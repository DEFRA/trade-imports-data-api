using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Services;

public class CustomsDeclarationService(IDbContext dbContext, IResourceEventPublisher resourceEventPublisher)
    : ICustomsDeclarationService
{
    public async Task<CustomsDeclarationEntity?> GetCustomsDeclaration(string mrn, CancellationToken cancellationToken)
    {
        return await dbContext.CustomsDeclarations.Find(mrn, cancellationToken);
    }

    public async Task<CustomsDeclarationEntity> Insert(
        CustomsDeclarationEntity customsDeclarationEntity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.CustomsDeclarations.Insert(customsDeclarationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await resourceEventPublisher.Publish(
            customsDeclarationEntity.ToResourceEvent(ResourceEventOperations.Created),
            cancellationToken
        );

        return customsDeclarationEntity;
    }

    public async Task<List<CustomsDeclarationEntity>> GetCustomsDeclarationsByChedId(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        var identifier = new ImportDocumentReference(chedId).GetIdentifier();

        return await dbContext
            .CustomsDeclarations.Where(x => x.ImportPreNotificationIdentifiers.Contains(identifier))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<CustomsDeclarationEntity> Update(
        CustomsDeclarationEntity customsDeclarationEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.CustomsDeclarations.Find(customsDeclarationEntity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(CustomsDeclarationEntity), customsDeclarationEntity.Id);
        }

        customsDeclarationEntity.Created = existing.Created;

        await dbContext.CustomsDeclarations.Update(customsDeclarationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await resourceEventPublisher.Publish(
            customsDeclarationEntity
                .ToResourceEvent(ResourceEventOperations.Updated)
                .WithChangeSet(
                    new CustomsDeclarationData(
                        customsDeclarationEntity.ClearanceRequest,
                        customsDeclarationEntity.ClearanceDecision,
                        customsDeclarationEntity.Finalisation
                    ),
                    new CustomsDeclarationData(
                        existing.ClearanceRequest,
                        existing.ClearanceDecision,
                        existing.Finalisation
                    )
                ),
            cancellationToken
        );

        return customsDeclarationEntity;
    }

    private sealed record CustomsDeclarationData(
        ClearanceRequest? ClearanceRequest,
        ClearanceDecision? ClearanceDecision,
        Finalisation? Finalisation
    );
}

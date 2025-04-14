using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Services;

public class CustomsDeclarationService(IDbContext dbContext) : ICustomsDeclarationService
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
        await dbContext.CustomsDeclarations.Update(customsDeclarationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return customsDeclarationEntity;
    }
}

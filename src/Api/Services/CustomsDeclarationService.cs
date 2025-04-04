using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class CustomsDeclarationService(IDbContext dbContext) : ICustomsDeclarationService
{
    public async Task<CustomsDeclarationEntity?> GetCustomsDeclaration(string mrn, CancellationToken cancellationToken)
    {
        return await dbContext.CustomDeclarations.Find(mrn, cancellationToken);
    }

    public async Task<CustomsDeclarationEntity> Insert(
        CustomsDeclarationEntity customsDeclarationEntity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.CustomDeclarations.Insert(customsDeclarationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return customsDeclarationEntity;
    }

    public async Task<CustomsDeclarationEntity> Update(
        CustomsDeclarationEntity customsDeclarationEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.CustomDeclarations.Update(customsDeclarationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return customsDeclarationEntity;
    }
}

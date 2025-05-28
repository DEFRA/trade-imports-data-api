using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public class GmrRepository(IDbContext dbContext) : IGmrRepository
{
    public Task<GmrEntity?> Get(string id, CancellationToken cancellationToken) =>
        dbContext.Gmrs.Find(id, cancellationToken);

    public async Task<List<GmrEntity>> GetAll(string[] customsDeclarationIds, CancellationToken cancellationToken) =>
        await dbContext.Gmrs.FindMany(
            x => x.CustomsDeclarationIdentifiers.Any(id => customsDeclarationIds.Any(cId => cId == id)),
            cancellationToken
        );

    public async Task<GmrEntity> Insert(GmrEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.Gmrs.Insert(entity, cancellationToken);

        return entity;
    }

    public async Task<(GmrEntity Existing, GmrEntity Updated)> Update(
        GmrEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.Gmrs.Find(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(GmrEntity), entity.Id);
        }

        entity.Created = existing.Created;

        await dbContext.Gmrs.Update(entity, etag, cancellationToken);

        return (existing, entity);
    }
}

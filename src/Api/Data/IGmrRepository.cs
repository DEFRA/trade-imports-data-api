using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IGmrRepository
{
    Task<GmrEntity?> Get(string id, CancellationToken cancellationToken);

    Task<GmrEntity?> Get(
        System.Linq.Expressions.Expression<Func<GmrEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    Task<List<GmrEntity>> GetAll(string[] customsDeclarationIds, CancellationToken cancellationToken);

    GmrEntity Insert(GmrEntity entity);

    Task<(GmrEntity Existing, GmrEntity Updated)> Update(
        GmrEntity entity,
        string etag,
        CancellationToken cancellationToken
    );
}

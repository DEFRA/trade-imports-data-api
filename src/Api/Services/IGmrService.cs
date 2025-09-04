using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IGmrService
{
    Task<GmrEntity?> GetGmr(string id, CancellationToken cancellationToken);

    Task<GmrEntity?> GetGmr(
        System.Linq.Expressions.Expression<Func<GmrEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    Task<List<GmrEntity>> GetGmrByChedId(string chedId, CancellationToken cancellationToken);

    Task<GmrEntity> Insert(GmrEntity entity, CancellationToken cancellationToken);

    Task<GmrEntity> Update(GmrEntity entity, string etag, CancellationToken cancellationToken);
}

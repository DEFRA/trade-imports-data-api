using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IGmrRepository
{
    Task<GmrEntity?> Get(string id, CancellationToken cancellationToken);

    Task<List<GmrEntity>> GetAll(string[] customsDeclarationIds, CancellationToken cancellationToken);

    Task<GmrEntity> Insert(GmrEntity entity, CancellationToken cancellationToken);

    Task<(GmrEntity Existing, GmrEntity Updated)> Update(
        GmrEntity entity,
        string etag,
        CancellationToken cancellationToken
    );
}

using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IGmrService
{
    Task<GmrEntity?> GetGmr(string id, CancellationToken cancellationToken);

    Task<List<GmrEntity>> GetGmrByChedId(string chedId, CancellationToken cancellationToken);

    Task<GmrEntity> Insert(GmrEntity gmrEntity, CancellationToken cancellationToken);

    Task<GmrEntity> Update(GmrEntity gmrEntity, string etag, CancellationToken cancellationToken);
}

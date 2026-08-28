using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface ITracesChedService
{
    Task<TracesChedEntity?> Get(string chedId, CancellationToken cancellationToken);

    Task<List<TracesChedEntity>> GetByMrn(string mrn, CancellationToken cancellationToken);

    Task<TracesChedUpdates> GetUpdates(TracesChedUpdateQuery query, CancellationToken cancellationToken);

    Task<TracesChedEntity> Insert(TracesChedEntity entity, CancellationToken cancellationToken);

    Task<TracesChedEntity> Update(TracesChedEntity entity, string etag, CancellationToken cancellationToken);
}

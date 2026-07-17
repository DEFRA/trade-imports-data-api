using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface ITracesChedService
{
    Task<TracesChedEntity?> Get(string chedId, CancellationToken cancellationToken);

    Task<TracesChedEntity> Insert(TracesChedEntity entity, CancellationToken cancellationToken);

    Task<TracesChedEntity> Update(TracesChedEntity entity, string etag, CancellationToken cancellationToken);
}

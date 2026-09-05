using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface ITracesChedRepository
{
    Task<TracesChedEntity?> Get(string id, CancellationToken cancellationToken);

    Task<TracesChedUpdates> GetUpdates(TracesChedUpdateQuery query, CancellationToken cancellationToken = default);

    Task<List<TracesChedEntity>> GetAll(string[] ids, CancellationToken cancellationToken);

    TracesChedEntity Insert(TracesChedEntity entity);

    Task<(TracesChedEntity Existing, TracesChedEntity Updated)> Update(
        TracesChedEntity entity,
        string etag,
        CancellationToken cancellationToken
    );
}

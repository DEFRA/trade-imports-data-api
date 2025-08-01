using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IProcessingErrorRepository
{
    Task<ProcessingErrorEntity?> Get(string id, CancellationToken cancellationToken);

    Task<ProcessingErrorEntity> Insert(ProcessingErrorEntity entity, CancellationToken cancellationToken);

    Task<(ProcessingErrorEntity Existing, ProcessingErrorEntity Updated)> Update(
        ProcessingErrorEntity entity,
        string etag,
        CancellationToken cancellationToken
    );
}

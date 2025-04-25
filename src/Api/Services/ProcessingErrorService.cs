using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ProcessingErrorService : IProcessingErrorService
{
    public Task<ProcessingErrorEntity?> GetProcessingError(string mrn, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ProcessingErrorEntity> Insert(
        ProcessingErrorEntity processingErrorEntity,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<ProcessingErrorEntity> Update(
        ProcessingErrorEntity processingErrorEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

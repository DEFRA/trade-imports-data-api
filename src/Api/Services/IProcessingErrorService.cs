using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IProcessingErrorService
{
    Task<ProcessingErrorEntity?> GetProcessingError(string mrn, CancellationToken cancellationToken);
    Task<ProcessingErrorEntity> Insert(
        ProcessingErrorEntity processingErrorEntity,
        CancellationToken cancellationToken
    );
    Task<ProcessingErrorEntity> Update(
        ProcessingErrorEntity processingErrorEntity,
        string etag,
        CancellationToken cancellationToken
    );
}

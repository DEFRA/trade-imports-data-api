using Defra.TradeImportsDataApi.Domain.ProcessingErrors;

namespace Defra.TradeImportsDataApi.Data.Entities;

public class ProcessingErrorEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public ProcessingError? ProcessingError { get; set; }

    public void OnSave() { }
}

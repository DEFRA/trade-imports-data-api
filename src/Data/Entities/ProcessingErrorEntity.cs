using Defra.TradeImportsDataApi.Domain.Errors;

namespace Defra.TradeImportsDataApi.Data.Entities;

public class ProcessingErrorEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required ProcessingError[] ProcessingErrors { get; set; }

    public void OnSave() { }
}

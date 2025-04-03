using Defra.TradeImportsData.Domain.Gvms;

namespace Defra.TradeImportsData.Data.Entities;

public class GmrEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required Gmr Data { get; set; }

    public void OnSave() { }
}

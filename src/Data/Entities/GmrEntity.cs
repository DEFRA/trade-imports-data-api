using Defra.TradeImportsDataApi.Domain.Gvms;

namespace Defra.TradeImportsDataApi.Data.Entities;

public class GmrEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required Gmr Data { get; set; }

    public void OnSave() { }
}

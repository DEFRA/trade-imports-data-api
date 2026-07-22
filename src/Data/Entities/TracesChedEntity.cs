using Defra.TradeImportsDataApi.Data.Configuration;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Data.Entities;

[DbCollection("TracesChed")]
public class TracesChedEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required DefraUNVTDCHEDProfile Ched { get; set; }

    public void OnSave() { }
}

using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class TracesChedEvent
{
    public required string Id { get; set; }

    public string Etag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required DefraUNVTDCHEDProfile Ched { get; set; }
}

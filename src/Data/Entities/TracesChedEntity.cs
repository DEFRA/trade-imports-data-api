using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Data.Entities;

[DbCollection("TracesChed")]
public class TracesChedEntity : ICustomsDeclarationIdentifierEntity
{
    public required string Id { get; set; }

    // This should not be used for matching against - it is only used for the max-id admin endpoint
    public string CustomsDeclarationIdentifier { get; set; } = null!;

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required DefraUNVTDCHEDProfile Ched { get; set; }

    public void OnSave()
    {
        CustomsDeclarationIdentifier = new ChedIdReference(Id).GetIdentifier();
    }
}

namespace Defra.TradeImportsDataApi.Api.Client;

public class RelatedImportDeclarationsRequest
{
    public string? Mrn { get; set; }

    public string? Ducr { get; set; }

    public string? ChedId { get; set; }

    public int? MaxLinkDepth { get; set; }
}

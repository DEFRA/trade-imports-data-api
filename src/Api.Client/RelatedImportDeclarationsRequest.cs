namespace Defra.TradeImportsDataApi.Api.Client;

public class RelatedImportDeclarationsRequest
{
    public string? Mrn { get; set; }

    public string? Ducr { get; set; }

    public string? ChedId { get; set; }

    public string? GmrId { get; set; }

    public string? Iuu { get; init; }

    public int? MaxLinkDepth { get; set; }
}

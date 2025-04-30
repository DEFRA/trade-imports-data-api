using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Search;

public class RelatedImportDeclarationsRequest
{
    [FromQuery(Name = "mrn")]
    public string? Mrn { get; set; }

    [FromQuery(Name = "ducr")]
    public string? Ducr { get; set; }

    [FromQuery(Name = "chedId")]
    public string? ChedId { get; set; }

    [FromQuery(Name = "maxLinkDepth")]
    public int? MaxLinkDepth { get; set; } = 3;
}

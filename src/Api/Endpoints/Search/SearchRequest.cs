using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Search;

public class SearchRequest
{
    [FromQuery]
    public string? Mrn { get; set; }

    [FromQuery]
    public string? Ducr { get; set; }

    [FromQuery]
    public string? ChedId { get; set; }

    [FromQuery]
    public string? Identifier { get; set; }
}
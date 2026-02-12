using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;

public class RelatedImportDeclarationsRequest
{
    [FromQuery(Name = "mrn")]
    [Description("Search by MRN")]
    public string? Mrn { get; init; }

    [FromQuery(Name = "ducr")]
    [Description("Search by DUCR")]
    public string? Ducr { get; init; }

    [FromQuery(Name = "chedId")]
    [Description("Search by CHED ID")]
    public string? ChedId { get; init; }

    [FromQuery(Name = "gmrId")]
    [Description("Search by GMR ID")]
    public string? GmrId { get; init; }

    [FromQuery(Name = "vrnOrTrn")]
    [Description("Search by GMR Vehicle or Trailer Registration")]
    public string? VrnOrTrn { get; init; }

    [FromQuery(Name = "maxLinkDepth")]
    [Description("Max link depth to follow. Default is 3.")]
    public int? MaxLinkDepth { get; init; } = 3;
}

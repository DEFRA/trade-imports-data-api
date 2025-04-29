using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Search;

public record SearchResponse(
    [property: JsonPropertyName("customsDeclarations")] CustomsDeclarationResponse[] CustomsDeclarations,
    [property: JsonPropertyName("importPreNotifications")] ImportPreNotificationResponse[] ImportPreNotifications
);

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

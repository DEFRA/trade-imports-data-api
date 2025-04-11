using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Gvms;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;

public record GmrResponse(
    [property: JsonPropertyName("gmr")] Gmr Gmr,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated
);

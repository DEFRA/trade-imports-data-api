using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record GmrResponse(
    [property: JsonPropertyName("gmr")] Domain.Gvms.Gmr Gmr,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);

using System.Text.Json.Serialization;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Api.Client;

public record TracesChedResponse(
    [property: JsonPropertyName("ched")] DefraUNVTDCHEDProfile Ched,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);

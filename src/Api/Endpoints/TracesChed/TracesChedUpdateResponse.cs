using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.TracesChed;

public record TracesChedUpdateResponse(
    [property: JsonPropertyName("referenceNumber")] string ReferenceNumber,
    [property: JsonPropertyName("updated")] DateTime Updated
);

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record TracesChedUpdatesResponse(
    [property: JsonPropertyName("tracesChedUpdates")] IReadOnlyList<TracesChedUpdateResponse> TracesChedUpdates,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("page")] int Page,
    [property: JsonPropertyName("pageSize")] int PageSize
);

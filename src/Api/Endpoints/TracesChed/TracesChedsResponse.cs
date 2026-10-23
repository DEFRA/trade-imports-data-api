using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.TracesChed;

public record TracesChedsResponse([property: JsonPropertyName("cheds")] IReadOnlyList<TracesChedResponse> Cheds);

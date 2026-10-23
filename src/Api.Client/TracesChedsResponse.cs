using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record TracesChedsResponse([property: JsonPropertyName("cheds")] IReadOnlyList<TracesChedResponse> Cheds);

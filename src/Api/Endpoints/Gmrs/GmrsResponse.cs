using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;

public record GmrsResponse([property: JsonPropertyName("gmrs")] IReadOnlyList<GmrResponse> Gmrs);
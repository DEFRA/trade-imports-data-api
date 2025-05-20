using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record GmrsResponse([property: JsonPropertyName("gmrs")] IReadOnlyList<GmrResponse> Gmrs);

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record CustomsDeclarationsResponse(
    [property: JsonPropertyName("customsDeclarations")] IReadOnlyList<CustomsDeclarationResponse> CustomsDeclarations
);

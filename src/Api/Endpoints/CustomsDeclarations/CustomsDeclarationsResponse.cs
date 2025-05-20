using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;

public record CustomsDeclarationsResponse(
    [property: JsonPropertyName("customsDeclarations")] IReadOnlyList<CustomsDeclarationResponse> CustomsDeclarations
);

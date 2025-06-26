using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Admin;

public record LatestResponse(
    [property: JsonPropertyName("importPreNotification")] string? ImportPreNotification,
    [property: JsonPropertyName("customsDeclaration")] string? CustomsDeclaration,
    [property: JsonPropertyName("gmr")] string? Gmr,
    [property: JsonPropertyName("processingError")] string? ProcessingError
);

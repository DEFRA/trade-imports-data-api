using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Errors;

namespace Defra.TradeImportsDataApi.Api.Client;

public record ProcessingErrorResponse(
    [property: JsonPropertyName("movementReferenceNumber")] string MovementReferenceNumber,
    [property: JsonPropertyName("processingErrors")] ProcessingError[] ProcessingErrors,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);

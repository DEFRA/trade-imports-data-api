using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.ProcessingErrors;

namespace Defra.TradeImportsDataApi.Api.Client;

public record ProcessingErrorResponse(
    [property: JsonPropertyName("movementReferenceNumber")] string MovementReferenceNumber,
    [property: JsonPropertyName("processingError")] ProcessingError ProcessingError,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);

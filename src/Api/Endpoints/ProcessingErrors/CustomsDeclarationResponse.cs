using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.ProcessingErrors;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ProcessingErrors;

public record ProcessingErrorResponse(
    [property: JsonPropertyName("movementReferenceNumber")] string MovementReferenceNumber,
    [property: JsonPropertyName("processingError")] ProcessingError? ProcessingError,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated
);

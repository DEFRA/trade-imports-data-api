using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Api.Client;

public record CustomsDeclarationResponse(
    [property: JsonPropertyName("movementReferenceNumber")] string MovementReferenceNumber,
    [property: JsonPropertyName("clearanceRequest")] ClearanceRequest? ClearanceRequest,
    [property: JsonPropertyName("clearanceDecision")] ClearanceDecision? ClearanceDecision,
    [property: JsonPropertyName("finalisation")] Finalisation? Finalisation,
    [property: JsonPropertyName("externalErrors")] ExternalError[]? ExternalErrors,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);

using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Api.Client;

public record CustomsDeclarationWithImportPreNotificationsResponse(
    [property: JsonPropertyName("movementReferenceNumber")] string MovementReferenceNumber,
    [property: JsonPropertyName("clearanceRequest")] ClearanceRequest? ClearanceRequest,
    [property: JsonPropertyName("clearanceDecision")] ClearanceDecision? ClearanceDecision,
    [property: JsonPropertyName("finalisation")] Finalisation? Finalisation,
    [property: JsonPropertyName("importPreNotifications")] List<ImportPreNotificationResponse> ImportPreNotifications,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated
);

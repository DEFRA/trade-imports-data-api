using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;

public record CustomsDeclarationWithImportPreNotificationsResponse(
    [property: JsonPropertyName("movementReferenceNumber")] string MovementReferenceNumber,
    [property: JsonPropertyName("clearanceRequest")] ClearanceRequest? ClearanceRequest,
    [property: JsonPropertyName("clearanceDecision")] ClearanceDecision? ClearanceDecision,
    [property: JsonPropertyName("finalisation")] Finalisation? Finalisation,
    [property: JsonPropertyName("importPreNotifications")] List<ImportPreNotificationResponse> ImportPreNotifications,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated
);

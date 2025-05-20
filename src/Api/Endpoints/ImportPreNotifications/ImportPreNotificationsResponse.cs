using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public record ImportPreNotificationsResponse(
    [property: JsonPropertyName("importPreNotifications")]
    IReadOnlyList<ImportPreNotificationResponse> ImportPreNotifications
);
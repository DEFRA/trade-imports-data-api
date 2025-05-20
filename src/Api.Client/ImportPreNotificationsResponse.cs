using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record ImportPreNotificationsResponse(
    [property: JsonPropertyName("importPreNotifications")]
        IReadOnlyList<ImportPreNotificationResponse> ImportPreNotifications
);

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public record ImportPreNotificationUpdatesResponse(
    [property: JsonPropertyName("importPreNotificationUpdates")]
        IReadOnlyList<ImportPreNotificationUpdateResponse> ImportPreNotificationUpdates
);

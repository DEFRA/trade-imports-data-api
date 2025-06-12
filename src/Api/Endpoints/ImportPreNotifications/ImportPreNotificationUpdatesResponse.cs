using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public record ImportPreNotificationUpdatesResponse(
    [property: JsonPropertyName("importPreNotificationUpdates")]
        IReadOnlyList<ImportPreNotificationUpdateResponse> ImportPreNotificationUpdates,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("page")] int Page,
    [property: JsonPropertyName("pageSize")] int PageSize
);

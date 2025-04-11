using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Client;

public record ImportPreNotificationResponse(
    [property: JsonPropertyName("importPreNotification")] Domain.Ipaffs.ImportPreNotification ImportPreNotification,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);

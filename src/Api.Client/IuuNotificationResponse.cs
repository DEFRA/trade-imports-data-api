using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Iuu;

namespace Defra.TradeImportsDataApi.Api.Client;

public record IuuNotificationResponse(
    [property: JsonPropertyName("iuuNotification")] IuuNotification IuuNotification,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("updated")] DateTime Updated,
    string? ETag = null
);

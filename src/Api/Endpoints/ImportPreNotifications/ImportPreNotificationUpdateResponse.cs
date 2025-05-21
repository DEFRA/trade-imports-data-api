using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public record ImportPreNotificationUpdateResponse(
    [property: JsonPropertyName("referenceNumber")] string ReferenceNumber,
    [property: JsonPropertyName("updated")] DateTime Updated
);

namespace Defra.TradeImportsDataApi.Api.Client;

public record ImportPreNotificationResponse(
    Domain.Ipaffs.ImportPreNotification ImportPreNotification,
    DateTime Created,
    DateTime Updated,
    string? ETag = null
);

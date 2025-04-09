namespace Defra.TradeImportsDataApi.Api.Client;

public record ImportNotificationResponse(
    Domain.Ipaffs.ImportPreNotification Data,
    DateTime Created,
    DateTime Updated,
    string? ETag = null
);

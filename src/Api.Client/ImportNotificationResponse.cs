namespace Defra.TradeImportsDataApi.Api.Client;

public record ImportNotificationResponse(
    Domain.Ipaffs.ImportNotification Data,
    DateTime Created,
    DateTime Updated,
    string? ETag = null
);

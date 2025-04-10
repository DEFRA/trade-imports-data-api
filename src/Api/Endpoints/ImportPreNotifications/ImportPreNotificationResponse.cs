using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public record ImportPreNotificationResponse(
    ImportPreNotification ImportPreNotification,
    DateTime Created,
    DateTime Updated
);

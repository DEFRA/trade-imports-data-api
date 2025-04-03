using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportNotifications;

public record ImportNotificationResponse(ImportNotification Data, DateTime Created, DateTime Updated);

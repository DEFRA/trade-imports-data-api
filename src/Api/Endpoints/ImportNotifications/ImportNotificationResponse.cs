using Defra.TradeImportsData.Domain.IPaffs;

namespace Defra.TradeImportsData.Api.Endpoints.ImportNotifications;

public record ImportNotificationResponse(ImportNotification Data, DateTime Created, DateTime Updated);

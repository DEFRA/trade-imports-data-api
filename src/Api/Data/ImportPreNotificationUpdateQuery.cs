namespace Defra.TradeImportsDataApi.Api.Data;

public record ImportPreNotificationUpdateQuery(
    DateTime From,
    DateTime To,
    string[]? PointOfEntry = null,
    string[]? Type = null,
    string[]? Status = null,
    string[]? ExcludeStatus = null,
    int Page = 1,
    int PageSize = 100
);

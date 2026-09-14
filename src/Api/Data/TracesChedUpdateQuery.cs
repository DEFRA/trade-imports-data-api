namespace Defra.TradeImportsDataApi.Api.Data;

public record TracesChedUpdateQuery(DateTime From, DateTime To, int Page = 1, int PageSize = 100);

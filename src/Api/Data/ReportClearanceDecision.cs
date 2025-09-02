namespace Defra.TradeImportsDataApi.Api.Data;

public record ReportClearanceDecision(DateTime Bucket, bool Match, int Count);

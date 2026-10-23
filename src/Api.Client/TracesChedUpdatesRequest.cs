namespace Defra.TradeImportsDataApi.Api.Client;

public class TracesChedUpdatesRequest
{
    public required DateTime From { get; set; }

    public required DateTime To { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

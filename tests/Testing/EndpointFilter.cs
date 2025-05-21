namespace Defra.TradeImportsDataApi.Testing;

public class EndpointFilter
{
    internal string Filter { get; }

    private EndpointFilter(string filter) => Filter = filter;

    public static EndpointFilter From(DateTime from) => new($"from={from:O}");

    public static EndpointFilter To(DateTime to) => new($"to={to:O}");
}

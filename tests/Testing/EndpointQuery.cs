namespace Defra.TradeImportsDataApi.Testing;

public class EndpointQuery
{
    private readonly EndpointFilter[] _filters = [];

    public EndpointQuery() { }

    public static EndpointQuery New => new();

    private EndpointQuery(EndpointFilter[] filters) => _filters = filters;

    public EndpointQuery Where(EndpointFilter filter) => new([.. _filters, filter]);

    public EndpointQuery Where(EndpointFilter[] filters) => new([.. _filters, .. filters]);

    public override string ToString() =>
        _filters.Length == 0 ? string.Empty : "?" + string.Join("&", _filters.Select(f => f.Filter));
}

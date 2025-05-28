namespace Defra.TradeImportsDataApi.Testing;

public class EndpointFilter
{
    internal string Filter { get; }

    private EndpointFilter(string filter) => Filter = filter;

    public static EndpointFilter From(DateTime from) => new($"from={from:O}");

    public static EndpointFilter To(DateTime to) => new($"to={to:O}");

    public static EndpointFilter[] PointOfEntry(string[]? pointsOfEntry) =>
        (pointsOfEntry ?? []).Select(PointOfEntry).ToArray();

    public static EndpointFilter PointOfEntry(string pointOfEntry) => new($"pointOfEntry={pointOfEntry}");

    public static EndpointFilter[] Type(string[]? types) => (types ?? []).Select(Type).ToArray();

    public static EndpointFilter Type(string type) => new($"type={type}");

    public static EndpointFilter[] Status(string[]? statuses) => (statuses ?? []).Select(Status).ToArray();

    public static EndpointFilter Status(string status) => new($"status={status}");
}

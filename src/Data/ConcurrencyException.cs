namespace Defra.TradeImportsDataApi.Data;

public class ConcurrencyException : Exception
{
    public ConcurrencyException(string entityId, string entityEtag)
        : base($"Failed up update {entityId} with etag {entityEtag}")
    {
        EntityId = entityId;
        EntityEtag = entityEtag;
    }

    public ConcurrencyException(string message, Exception inner)
        : base(message, inner) { }

    public string? EntityId { get; }

    public string? EntityEtag { get; }
}

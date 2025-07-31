namespace Defra.TradeImportsDataApi.Data.Entities;

public class ResourceEventEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required string ResourceId { get; set; }

    public required string ResourceType { get; set; }

    public required object ResourceEvent { get; set; }

    public DateTime? Published { get; set; }

    public void OnSave() { }
}

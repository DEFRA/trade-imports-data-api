namespace Defra.TradeImportsDataApi.Data.Entities;

public class ReportClearanceDecisionEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required string MovementReferenceNumber { get; set; }

    public bool Match { get; set; }

    public void OnSave() { }
}

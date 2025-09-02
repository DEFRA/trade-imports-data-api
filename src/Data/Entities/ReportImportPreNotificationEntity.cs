namespace Defra.TradeImportsDataApi.Data.Entities;

public class ReportImportPreNotificationEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required string ImportPreNotificationId { get; set; }

    public string? ImportNotificationType { get; set; }

    public void OnSave() { }
}

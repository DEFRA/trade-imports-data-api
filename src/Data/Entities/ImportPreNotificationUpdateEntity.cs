namespace Defra.TradeImportsDataApi.Data.Entities;

public class ImportPreNotificationUpdateEntity : IDataEntity
{
    private IDataEntity? _source;

    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required string ImportPreNotificationId { get; set; }

    public string? PointOfEntry { get; set; }

    public string? ImportNotificationType { get; set; }

    public string? Status { get; set; }

    public Source? Source { get; set; }

    public void OnSave()
    {
        if (_source != null)
        {
            Source = new Source
            {
                Id = _source.Id,
                Type = _source.GetType().Name,
                Updated = _source.Updated,
            };
        }
    }

    public void SetSource(IDataEntity source) => _source = source;
}

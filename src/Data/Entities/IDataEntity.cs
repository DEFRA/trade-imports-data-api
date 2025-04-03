namespace Defra.TradeImportsData.Data.Entities;

public interface IDataEntity
{
    public string Id { get; set; }

    // ReSharper disable once InconsistentNaming - want to use Mongo DB convention to indicate none core schema properties
    public string ETag { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    void OnSave();
}

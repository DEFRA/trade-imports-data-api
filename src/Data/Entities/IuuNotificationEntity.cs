using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Domain.Iuu;

namespace Defra.TradeImportsDataApi.Data.Entities;

[DbCollection("IuuNotification")]
public class IuuNotificationEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required IuuNotification IuuNotification { get; set; }

    public List<string> Tags { get; set; } = [];

    public void OnSave()
    {
        var lowerId = Id.ToLower();
        if (!Tags.Contains(lowerId))
        {
            Tags.Add(lowerId);
        }
    }
}

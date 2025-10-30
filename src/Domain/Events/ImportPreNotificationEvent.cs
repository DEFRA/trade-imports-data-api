using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class ImportPreNotificationEvent
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("etag")]
    public string ETag { get; set; } = null!;

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }

    [JsonPropertyName("importPreNotification")]
    public required ImportPreNotification ImportPreNotification { get; set; }
}

using Defra.TradeImportsDataApi.Domain.Iuu;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class IuuNotificationEvent
{
    public required string Id { get; set; }

    public string Etag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required IuuNotification IuuNotification { get; set; }
}

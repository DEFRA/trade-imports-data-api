using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class InboundError
{
    [JsonPropertyName("notifications")]
    public InboundErrorNotification[]? Notifications { get; set; }
}

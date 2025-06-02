using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Errors;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ExternalError
{
    [JsonPropertyName("notifications")]
    public ErrorNotification[]? Notifications { get; set; }
}

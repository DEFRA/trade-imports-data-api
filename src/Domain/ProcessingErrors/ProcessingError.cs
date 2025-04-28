using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Errors;

namespace Defra.TradeImportsDataApi.Domain.ProcessingErrors;

public class ProcessingError
{
    [JsonPropertyName("notifications")]
    public ErrorNotification[]? Notifications { get; set; }
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Errors;

public class ErrorItem
{
    [JsonPropertyName("code")]
    public string? Code { get; set; } = null!;

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Gvms;

public class ErrorResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
}

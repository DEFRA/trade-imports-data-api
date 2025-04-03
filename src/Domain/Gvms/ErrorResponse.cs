using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.Gvms;

public class ErrorResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
}

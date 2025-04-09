using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class Diff
{
    [JsonPropertyName("path")]
    public required string Path { get; set; }

    [JsonPropertyName("operation")]
    public required string Operation { get; set; }

    [JsonPropertyName("Value")]
    public string? Value { get; set; }
}
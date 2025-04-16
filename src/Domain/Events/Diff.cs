using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Events;

public record Diff
{
    [JsonPropertyName("path")]
    public required string Path { get; init; }

    [JsonPropertyName("operation")]
    public required string Operation { get; init; }

    [JsonPropertyName("value")]
    public string? Value { get; init; }
}

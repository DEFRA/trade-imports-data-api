using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Iuu;

public class IuuNotification
{
    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("lastUpdated")]
    public required DateTime LastUpdated { get; set; }

    [JsonPropertyName("iuuOption")]
    [PossibleValue("IUUOK")]
    [PossibleValue("IUUNA")]
    [PossibleValue("IUUNotCompliant")]
    public string? IuuOption { get; set; }
}

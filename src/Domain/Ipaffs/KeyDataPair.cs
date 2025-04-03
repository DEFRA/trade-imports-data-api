using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

public class KeyDataPair
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

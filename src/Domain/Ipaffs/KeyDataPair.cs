using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class KeyDataPair
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

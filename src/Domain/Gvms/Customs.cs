using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Gvms;

public class Customs
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

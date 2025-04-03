using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Seal container details
/// </summary>
public class SealContainer
{
    [JsonPropertyName("sealNumber")]
    public string? SealNumber { get; set; }

    [JsonPropertyName("containerNumber")]
    public string? ContainerNumber { get; set; }

    [JsonPropertyName("officialSeal")]
    public bool? OfficialSeal { get; set; }

    [JsonPropertyName("resealedSealNumber")]
    public string? ResealedSealNumber { get; set; }
}

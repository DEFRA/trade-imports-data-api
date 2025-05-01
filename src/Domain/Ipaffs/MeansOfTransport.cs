using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Details of transport
/// </summary>
public class MeansOfTransport
{
    /// <summary>
    /// Type of transport
    /// </summary>
    [JsonPropertyName("type")]
    [System.ComponentModel.Description("Type of transport")]
    [PossibleValue("Aeroplane")]
    [PossibleValue("Road Vehicle")]
    [PossibleValue("Railway Wagon")]
    [PossibleValue("Ship")]
    [PossibleValue("Other")]
    [PossibleValue("Road vehicle Aeroplane")]
    [PossibleValue("Ship Railway wagon")]
    [PossibleValue("Ship Road vehicle")]
    public string? Type { get; set; }

    /// <summary>
    /// Document for transport
    /// </summary>
    [JsonPropertyName("document")]
    [System.ComponentModel.Description("Document for transport")]
    public string? Document { get; set; }

    /// <summary>
    /// ID of transport
    /// </summary>
    [JsonPropertyName("id")]
    [System.ComponentModel.Description("ID of transport")]
    public string? Id { get; set; }
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Impact of transport on animals
/// </summary>
public class ImpactOfTransportOnAnimals
{
    /// <summary>
    /// Number of dead animals specified by units
    /// </summary>
    [JsonPropertyName("numberOfDeadAnimals")]
    [System.ComponentModel.Description("Number of dead animals specified by units")]
    public int? NumberOfDeadAnimals { get; set; }

    /// <summary>
    /// Unit used for specifying number of dead animals (percent or units)
    /// </summary>
    [JsonPropertyName("numberOfDeadAnimalsUnit")]
    [System.ComponentModel.Description("Unit used for specifying number of dead animals (percent or units)")]
    public string? NumberOfDeadAnimalsUnit { get; set; }

    /// <summary>
    /// Number of unfit animals
    /// </summary>
    [JsonPropertyName("numberOfUnfitAnimals")]
    [System.ComponentModel.Description("Number of unfit animals")]
    public int? NumberOfUnfitAnimals { get; set; }

    /// <summary>
    /// Unit used for specifying number of unfit animals (percent or units)
    /// </summary>
    [JsonPropertyName("numberOfUnfitAnimalsUnit")]
    [System.ComponentModel.Description("Unit used for specifying number of unfit animals (percent or units)")]
    public string? NumberOfUnfitAnimalsUnit { get; set; }

    /// <summary>
    /// Number of births or abortions (unit)
    /// </summary>
    [JsonPropertyName("numberOfBirthOrAbortion")]
    [System.ComponentModel.Description("Number of births or abortions (unit)")]
    public int? NumberOfBirthOrAbortion { get; set; }
}

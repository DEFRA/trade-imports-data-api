using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Authority which performed control
/// </summary>
public class ControlAuthority
{
    /// <summary>
    /// Official veterinarian
    /// </summary>

    [JsonPropertyName("officialVeterinarian")]
    [System.ComponentModel.Description("Official veterinarian")]
    public OfficialVeterinarian? OfficialVeterinarian { get; set; }

    /// <summary>
    /// Customs reference number
    /// </summary>

    [JsonPropertyName("customsReferenceNo")]
    [System.ComponentModel.Description("Customs reference number")]
    public string? CustomsReferenceNo { get; set; }

    /// <summary>
    /// Were containers resealed?
    /// </summary>

    [JsonPropertyName("containerResealed")]
    [System.ComponentModel.Description("Were containers resealed?")]
    public bool? ContainerResealed { get; set; }

    /// <summary>
    /// When the containers are resealed they need new seal numbers
    /// </summary>

    [JsonPropertyName("newSealNumber")]
    [System.ComponentModel.Description("When the containers are resealed they need new seal numbers")]
    public string? NewSealNumber { get; set; }

    /// <summary>
    /// Illegal, Unreported and Unregulated (IUU) fishing reference number
    /// </summary>

    [JsonPropertyName("iuuFishingReference")]
    [System.ComponentModel.Description("Illegal, Unreported and Unregulated (IUU) fishing reference number")]
    public string? IuuFishingReference { get; set; }

    /// <summary>
    /// Was Illegal, Unreported and Unregulated (IUU) check required
    /// </summary>

    [JsonPropertyName("iuuCheckRequired")]
    [System.ComponentModel.Description("Was Illegal, Unreported and Unregulated (IUU) check required")]
    public bool? IuuCheckRequired { get; set; }

    /// <summary>
    /// Result of Illegal, Unreported and Unregulated (IUU) check
    /// </summary>

    [JsonPropertyName("iuuOption")]
    [System.ComponentModel.Description("Result of Illegal, Unreported and Unregulated (IUU) check")]
    public ControlAuthorityIuuOption? IuuOption { get; set; }
}

using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Control part of notification
/// </summary>
public class PartThree
{
    /// <summary>
    /// Control status enum
    /// </summary>
    [JsonPropertyName("controlStatus")]
    [System.ComponentModel.Description("Control status enum")]
    [PossibleValue("REQUIRED")]
    [PossibleValue("COMPLETED")]
    public string? ControlStatus { get; set; }

    /// <summary>
    /// Control details
    /// </summary>
    [JsonPropertyName("control")]
    [System.ComponentModel.Description("Control details")]
    public Control? Control { get; set; }

    /// <summary>
    /// Validation messages for Part 3 - Control
    /// </summary>
    [JsonPropertyName("consignmentValidations")]
    [System.ComponentModel.Description("Validation messages for Part 3 - Control")]
    public ValidationMessageCode[]? ConsignmentValidations { get; set; }

    /// <summary>
    /// Is the seal check required
    /// </summary>
    [JsonPropertyName("sealCheckRequired")]
    [System.ComponentModel.Description("Is the seal check required")]
    public bool? SealCheckRequired { get; set; }

    /// <summary>
    /// Seal check details
    /// </summary>
    [JsonPropertyName("sealCheck")]
    [System.ComponentModel.Description("Seal check details")]
    public SealCheck? SealCheck { get; set; }

    /// <summary>
    /// Seal check override details
    /// </summary>
    [JsonPropertyName("sealCheckOverride")]
    [System.ComponentModel.Description("Seal check override details")]
    public InspectionOverride? SealCheckOverride { get; set; }
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Details about the manual inspection override
/// </summary>
public class InspectionOverride
{
    /// <summary>
    /// Original inspection decision
    /// </summary>
    [JsonPropertyName("originalDecision")]
    [System.ComponentModel.Description("Original inspection decision")]
    public string? OriginalDecision { get; set; }

    /// <summary>
    /// The time the risk decision is overridden
    /// </summary>
    [JsonPropertyName("overriddenOn")]
    [System.ComponentModel.Description("The time the risk decision is overridden")]
    public DateTime? OverriddenOn { get; set; }

    /// <summary>
    /// User entity who has manually overridden the inspection
    /// </summary>
    [JsonPropertyName("overriddenBy")]
    [System.ComponentModel.Description("User entity who has manually overridden the inspection")]
    public UserInformation? OverriddenBy { get; set; }
}

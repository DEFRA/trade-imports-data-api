using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// inspector
/// </summary>
public class Inspector
{
    /// <summary>
    /// Name of inspector
    /// </summary>

    [JsonPropertyName("name")]
    [System.ComponentModel.Description("Name of inspector")]
    public string? Name { get; set; }

    /// <summary>
    /// Phone number of inspector
    /// </summary>

    [JsonPropertyName("phone")]
    [System.ComponentModel.Description("Phone number of inspector")]
    public string? Phone { get; set; }

    /// <summary>
    /// Email address of inspector
    /// </summary>

    [JsonPropertyName("email")]
    [System.ComponentModel.Description("Email address of inspector")]
    public string? Email { get; set; }
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Person to be nominated for text and email contact for the consignment
/// </summary>
public class NominatedContact
{
    /// <summary>
    /// Name of nominated contact
    /// </summary>
    [JsonPropertyName("name")]
    [System.ComponentModel.Description("Name of nominated contact")]
    public string? Name { get; set; }

    /// <summary>
    /// Email address of nominated contact
    /// </summary>
    [JsonPropertyName("email")]
    [System.ComponentModel.Description("Email address of nominated contact")]
    public string? Email { get; set; }

    /// <summary>
    /// Telephone number of nominated contact
    /// </summary>
    [JsonPropertyName("telephone")]
    [System.ComponentModel.Description("Telephone number of nominated contact")]
    public string? Telephone { get; set; }
}

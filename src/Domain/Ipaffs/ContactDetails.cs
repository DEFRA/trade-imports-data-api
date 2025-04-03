using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Person to be contacted if there is an issue with the consignment
/// </summary>
public class ContactDetails
{
    /// <summary>
    /// Name of designated contact
    /// </summary>

    [JsonPropertyName("name")]
    [System.ComponentModel.Description("Name of designated contact")]
    public string? Name { get; set; }

    /// <summary>
    /// Telephone number of designated contact
    /// </summary>

    [JsonPropertyName("telephone")]
    [System.ComponentModel.Description("Telephone number of designated contact")]
    public string? Telephone { get; set; }

    /// <summary>
    /// Email address of designated contact
    /// </summary>

    [JsonPropertyName("email")]
    [System.ComponentModel.Description("Email address of designated contact")]
    public string? Email { get; set; }

    /// <summary>
    /// Name of agent representing designated contact
    /// </summary>

    [JsonPropertyName("agent")]
    [System.ComponentModel.Description("Name of agent representing designated contact")]
    public string? Agent { get; set; }
}

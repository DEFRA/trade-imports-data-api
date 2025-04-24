using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Official veterinarian information
/// </summary>
public class OfficialVeterinarian
{
    /// <summary>
    /// First name of official veterinarian
    /// </summary>
    [JsonPropertyName("firstName")]
    [System.ComponentModel.Description("First name of official veterinarian")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name of official veterinarian
    /// </summary>
    [JsonPropertyName("lastName")]
    [System.ComponentModel.Description("Last name of official veterinarian")]
    public string? LastName { get; set; }

    /// <summary>
    /// Email address of official veterinarian
    /// </summary>
    [JsonPropertyName("email")]
    [System.ComponentModel.Description("Email address of official veterinarian")]
    public string? Email { get; set; }

    /// <summary>
    /// Phone number of official veterinarian
    /// </summary>
    [JsonPropertyName("phone")]
    [System.ComponentModel.Description("Phone number of official veterinarian")]
    public string? Phone { get; set; }

    /// <summary>
    /// Fax number of official veterinarian
    /// </summary>
    [JsonPropertyName("fax")]
    [System.ComponentModel.Description("Fax number of official veterinarian")]
    public string? Fax { get; set; }

    /// <summary>
    /// Date of sign
    /// </summary>
    [JsonPropertyName("signed")]
    [System.ComponentModel.Description("Date of sign")]
    public string? Signed { get; set; }
}

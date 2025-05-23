using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Party details
/// </summary>
public class Party
{
    /// <summary>
    /// IPAFFS ID of party
    /// </summary>
    [JsonPropertyName("id")]
    [System.ComponentModel.Description("IPAFFS ID of party")]
    public string? Id { get; set; }

    /// <summary>
    /// Name of party
    /// </summary>
    [JsonPropertyName("name")]
    [System.ComponentModel.Description("Name of party")]
    public string? Name { get; set; }

    /// <summary>
    /// Company ID
    /// </summary>
    [JsonPropertyName("companyId")]
    [System.ComponentModel.Description("Company ID")]
    public string? CompanyId { get; set; }

    /// <summary>
    /// Contact ID (B2C)
    /// </summary>
    [JsonPropertyName("contactId")]
    [System.ComponentModel.Description("Contact ID (B2C)")]
    public string? ContactId { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    [JsonPropertyName("companyName")]
    [System.ComponentModel.Description("Company name")]
    public string? CompanyName { get; set; }

    /// <summary>
    /// Addresses
    /// </summary>
    [JsonPropertyName("addresses")]
    [System.ComponentModel.Description("Addresses")]
    public string[]? Addresses { get; set; }

    /// <summary>
    /// County
    /// </summary>
    [JsonPropertyName("county")]
    [System.ComponentModel.Description("County")]
    public string? County { get; set; }

    /// <summary>
    /// Post code of party
    /// </summary>
    [JsonPropertyName("postCode")]
    [System.ComponentModel.Description("Post code of party")]
    public string? PostCode { get; set; }

    /// <summary>
    /// Country of party
    /// </summary>
    [JsonPropertyName("country")]
    [System.ComponentModel.Description("Country of party")]
    public string? Country { get; set; }

    /// <summary>
    /// City
    /// </summary>
    [JsonPropertyName("city")]
    [System.ComponentModel.Description("City")]
    public string? City { get; set; }

    /// <summary>
    /// TRACES ID
    /// </summary>
    [JsonPropertyName("tracesId")]
    [System.ComponentModel.Description("TRACES ID")]
    public int? TracesId { get; set; }

    /// <summary>
    /// Type of party
    /// </summary>
    [JsonPropertyName("type")]
    [System.ComponentModel.Description("Type of party")]
    [PossibleValue("Commercial transporter")]
    [PossibleValue("Private transporter")]
    public string? Type { get; set; }

    /// <summary>
    /// Approval number
    /// </summary>
    [JsonPropertyName("approvalNumber")]
    [System.ComponentModel.Description("Approval number")]
    public string? ApprovalNumber { get; set; }

    /// <summary>
    /// Phone number of party
    /// </summary>
    [JsonPropertyName("phone")]
    [System.ComponentModel.Description("Phone number of party")]
    public string? Phone { get; set; }

    /// <summary>
    /// Fax number of party
    /// </summary>
    [JsonPropertyName("fax")]
    [System.ComponentModel.Description("Fax number of party")]
    public string? Fax { get; set; }

    /// <summary>
    /// Email number of party
    /// </summary>
    [JsonPropertyName("email")]
    [System.ComponentModel.Description("Email number of party")]
    public string? Email { get; set; }
}

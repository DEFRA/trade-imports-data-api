using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class BillingInformation
{
    /// <summary>
    /// Indicates whether user has confirmed their billing information
    /// </summary>

    [JsonPropertyName("isConfirmed")]
    [System.ComponentModel.Description("Indicates whether user has confirmed their billing information")]
    public bool? IsConfirmed { get; set; }

    /// <summary>
    /// Billing email address
    /// </summary>

    [JsonPropertyName("emailAddress")]
    [System.ComponentModel.Description("Billing email address")]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Billing phone number
    /// </summary>

    [JsonPropertyName("phoneNumber")]
    [System.ComponentModel.Description("Billing phone number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Billing Contact Name
    /// </summary>

    [JsonPropertyName("contactName")]
    [System.ComponentModel.Description("Billing Contact Name")]
    public string? ContactName { get; set; }

    /// <summary>
    /// Billing postal address
    /// </summary>

    [JsonPropertyName("postalAddress")]
    [System.ComponentModel.Description("Billing postal address")]
    public PostalAddress? PostalAddress { get; set; }
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Inspector Address
/// </summary>
public class Address
{
    /// <summary>
    /// Street
    /// </summary>
    [JsonPropertyName("street")]
    [System.ComponentModel.Description("Street")]
    public string? Street { get; set; }

    /// <summary>
    /// City
    /// </summary>
    [JsonPropertyName("city")]
    [System.ComponentModel.Description("City")]
    public string? City { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    [JsonPropertyName("country")]
    [System.ComponentModel.Description("Country")]
    public string? Country { get; set; }

    /// <summary>
    /// Postal Code
    /// </summary>
    [JsonPropertyName("postalCode")]
    [System.ComponentModel.Description("Postal Code")]
    public string? PostalCode { get; set; }

    /// <summary>
    /// 1st line of address
    /// </summary>
    [JsonPropertyName("addressLine1")]
    [System.ComponentModel.Description("1st line of address")]
    public string? AddressLine1 { get; set; }

    /// <summary>
    /// 2nd line of address
    /// </summary>
    [JsonPropertyName("addressLine2")]
    [System.ComponentModel.Description("2nd line of address")]
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// 3rd line of address
    /// </summary>
    [JsonPropertyName("addressLine3")]
    [System.ComponentModel.Description("3rd line of address")]
    public string? AddressLine3 { get; set; }

    /// <summary>
    /// Post / zip code
    /// </summary>
    [JsonPropertyName("postalZipCode")]
    [System.ComponentModel.Description("Post / zip code")]
    public string? PostalZipCode { get; set; }

    /// <summary>
    /// country 2-digits ISO code
    /// </summary>
    [JsonPropertyName("countryIsoCode")]
    [System.ComponentModel.Description("country 2-digits ISO code")]
    public string? CountryIsoCode { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    [JsonPropertyName("email")]
    [System.ComponentModel.Description("Email address")]
    public string? Email { get; set; }

    /// <summary>
    /// UK phone number
    /// </summary>
    [JsonPropertyName("ukTelephone")]
    [System.ComponentModel.Description("UK phone number")]
    public string? UkTelephone { get; set; }

    /// <summary>
    /// Telephone number
    /// </summary>
    [JsonPropertyName("telephone")]
    [System.ComponentModel.Description("Telephone number")]
    public string? Telephone { get; set; }

    /// <summary>
    /// International phone number
    /// </summary>
    [JsonPropertyName("internationalTelephone")]
    [System.ComponentModel.Description("International phone number")]
    public InternationalTelephone? InternationalTelephone { get; set; }
}

using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Billing postal address
/// </summary>
public class PostalAddress
{
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
    /// 4th line of address
    /// </summary>

    [JsonPropertyName("addressLine4")]
    [System.ComponentModel.Description("4th line of address")]
    public string? AddressLine4 { get; set; }

    /// <summary>
    /// 3rd line of address
    /// </summary>

    [JsonPropertyName("county")]
    [System.ComponentModel.Description("3rd line of address")]
    public string? County { get; set; }

    /// <summary>
    /// City or town name
    /// </summary>

    [JsonPropertyName("cityOrTown")]
    [System.ComponentModel.Description("City or town name")]
    public string? CityOrTown { get; set; }

    /// <summary>
    /// Post code
    /// </summary>

    [JsonPropertyName("postalCode")]
    [System.ComponentModel.Description("Post code")]
    public string? PostalCode { get; set; }
}

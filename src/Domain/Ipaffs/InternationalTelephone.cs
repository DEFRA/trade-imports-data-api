using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// International phone number
/// </summary>
public class InternationalTelephone
{
    /// <summary>
    /// Country code of phone number
    /// </summary>

    [JsonPropertyName("countryCode")]
    [System.ComponentModel.Description("Country code of phone number")]
    public string? CountryCode { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>

    [JsonPropertyName("subscriberNumber")]
    [System.ComponentModel.Description("Phone number")]
    public string? SubscriberNumber { get; set; }
}

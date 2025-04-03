using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Information about logged-in user
/// </summary>
public class UserInformation
{
    /// <summary>
    /// Display name
    /// </summary>

    [JsonPropertyName("displayName")]
    [System.ComponentModel.Description("Display name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// User ID
    /// </summary>

    [JsonPropertyName("userId")]
    [System.ComponentModel.Description("User ID")]
    public string? UserId { get; set; }

    /// <summary>
    /// Is this user a control
    /// </summary>

    [JsonPropertyName("isControlUser")]
    [System.ComponentModel.Description("Is this user a control")]
    public bool? IsControlUser { get; set; }
}

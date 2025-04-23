using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Validation field code-message representation
/// </summary>
public class ValidationMessageCode
{
    /// <summary>
    /// Field
    /// </summary>
    [JsonPropertyName("field")]
    [System.ComponentModel.Description("Field")]
    public string? Field { get; set; }

    /// <summary>
    /// Code
    /// </summary>
    [JsonPropertyName("code")]
    [System.ComponentModel.Description("Code")]
    public string? Code { get; set; }
}

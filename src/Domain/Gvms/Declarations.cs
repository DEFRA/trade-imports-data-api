using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.Gvms;

public partial class Declarations
{
    /// <summary>
    /// A list of declaration ids.
    /// </summary>

    [JsonPropertyName("transits")]
    [System.ComponentModel.Description("A list of declaration ids.")]
    public Transits[]? Transits { get; set; }

    /// <summary>
    /// A list of declaration ids.
    /// </summary>

    [JsonPropertyName("customs")]
    [System.ComponentModel.Description("A list of declaration ids.")]
    public Customs[]? Customs { get; set; }
}

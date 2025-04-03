using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

public class CommonUserCharge
{
    /// <summary>
    /// Indicates whether the last applicable change was successfully send over the interface to Trade Charge
    /// </summary>

    [JsonPropertyName("wasSentToTradeCharge")]
    [System.ComponentModel.Description(
        "Indicates whether the last applicable change was successfully send over the interface to Trade Charge"
    )]
    public bool? WasSentToTradeCharge { get; set; }
}

using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Information about not acceptable reason
/// </summary>
public class ChedppNotAcceptableReason
{
    /// <summary>
    /// reason for refusal
    /// </summary>
    [JsonPropertyName("reason")]
    [System.ComponentModel.Description("reason for refusal")]
    [PossibleValue("doc-phmdm")]
    [PossibleValue("doc-phmdii")]
    [PossibleValue("doc-pa")]
    [PossibleValue("doc-pic")]
    [PossibleValue("doc-pill")]
    [PossibleValue("doc-ped")]
    [PossibleValue("doc-pmod")]
    [PossibleValue("doc-pfi")]
    [PossibleValue("doc-pnol")]
    [PossibleValue("doc-pcne")]
    [PossibleValue("doc-padm")]
    [PossibleValue("doc-padi")]
    [PossibleValue("doc-ppni")]
    [PossibleValue("doc-pf")]
    [PossibleValue("doc-po")]
    [PossibleValue("doc-ncevd")]
    [PossibleValue("doc-ncpqefi")]
    [PossibleValue("doc-ncpqebec")]
    [PossibleValue("doc-ncts")]
    [PossibleValue("doc-nco")]
    [PossibleValue("doc-orii")]
    [PossibleValue("doc-orsr")]
    [PossibleValue("ori-orrnu")]
    [PossibleValue("phy-orpp")]
    [PossibleValue("phy-orho")]
    [PossibleValue("phy-is")]
    [PossibleValue("phy-orsr")]
    [PossibleValue("oth-cnl")]
    [PossibleValue("oth-o")]
    public string? Reason { get; set; }

    /// <summary>
    /// commodity or package
    /// </summary>
    [JsonPropertyName("commodityOrPackage")]
    [System.ComponentModel.Description("commodity or package")]
    [PossibleValue("c")]
    [PossibleValue("p")]
    [PossibleValue("cp")]
    public string? CommodityOrPackage { get; set; }
}

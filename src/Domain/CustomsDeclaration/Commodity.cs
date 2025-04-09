using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class Commodity
{
    [JsonPropertyName("itemNumber")]
    public int? ItemNumber { get; set; }

    [JsonPropertyName("customsProcedureCode")]
    public string? CustomsProcedureCode { get; set; }

    [JsonPropertyName("taricCommodityCode")]
    public string? TaricCommodityCode { get; set; }

    [JsonPropertyName("goodsDescription")]
    public string? GoodsDescription { get; set; }

    [JsonPropertyName("consigneeId")]
    public string? ConsigneeId { get; set; }

    [JsonPropertyName("consigneeName")]
    public string? ConsigneeName { get; set; }

    [JsonPropertyName("netMass")]
    public decimal? NetMass { get; set; }

    [JsonPropertyName("supplementaryUnits")]
    public decimal? SupplementaryUnits { get; set; }

    [JsonPropertyName("thirdQuantity")]
    public decimal? ThirdQuantity { get; set; }

    [JsonPropertyName("originCountryCode")]
    public string? OriginCountryCode { get; set; }

    [JsonPropertyName("documents")]
    public ImportDocument[]? Documents { get; set; }

    [JsonPropertyName("checks")]
    public CommodityCheck[]? Checks { get; set; }
}

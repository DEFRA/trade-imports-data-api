using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.CustomsDeclaration.ClearanceRequest;

public class Item
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

    [JsonPropertyName("itemNetMass")]
    public decimal? ItemNetMass { get; set; }

    [JsonPropertyName("itemSupplementaryUnits")]
    public decimal? ItemSupplementaryUnits { get; set; }

    [JsonPropertyName("itemThirdQuantity")]
    public decimal? ItemThirdQuantity { get; set; }

    [JsonPropertyName("itemOriginCountryCode")]
    public string? ItemOriginCountryCode { get; set; }

    [JsonPropertyName("documents")]
    public Document[]? Documents { get; set; }

    [JsonPropertyName("checks")]
    public Check[]? Checks { get; set; }
}

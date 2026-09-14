using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Traces;

public class ReservationCommodity
{
    [JsonPropertyName("goodsItemNumber")]
    public required int GoodsItemNumber { get; set; }

    [JsonPropertyName("commodityCode")]
    public required string CommodityCode { get; set; }

    [JsonPropertyName("certificateLineNumber")]
    public required int CertificateLineNumber { get; set; }

    [JsonPropertyName("unitOfMeasure")]
    public required string UnitOfMeasure { get; set; }

    [JsonPropertyName("quantity")]
    public required decimal Quantity { get; set; }
}

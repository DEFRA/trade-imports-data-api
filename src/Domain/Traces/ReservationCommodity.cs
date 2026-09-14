using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Traces;

public class ReservationCommodity
{
    public required int GoodsItemNumber { get; set; }
    public required string CommodityCode { get; set; }
    public required int CertificateLineNumber { get; set; }
    public required string UnitOfMeasure { get; set; }
    public required decimal Quantity { get; set; }
}

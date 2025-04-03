using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommodityRiskResultExitRiskDecision
{
    Required,

    Notrequired,

    Inconclusive,
}

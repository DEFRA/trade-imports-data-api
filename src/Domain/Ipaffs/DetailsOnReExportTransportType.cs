using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DetailsOnReExportTransportType
{
    Rail,

    Plane,

    Ship,

    Road,

    Other,

    CShipRoad,

    CShipRail,
}

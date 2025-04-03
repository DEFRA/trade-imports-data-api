using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MeansOfTransportType
{
    Aeroplane,

    RoadVehicle,

    RailwayWagon,

    Ship,

    Other,

    RoadVehicleAeroplane,

    ShipRailwayWagon,

    ShipRoadVehicle,
}

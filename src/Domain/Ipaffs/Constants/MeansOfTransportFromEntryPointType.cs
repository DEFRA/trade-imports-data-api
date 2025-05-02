namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class MeansOfTransportFromEntryPointType
{
    public const string Aeroplane = "Aeroplane";
    public const string RoadVehicle = "Road Vehicle";
    public const string RailwayWagon = "Railway Wagon";
    public const string Ship = "Ship";
    public const string Other = "Other";
    public const string RoadVehicleAeroplane = "Road vehicle Aeroplane";
    public const string ShipRailwayWagon = "Ship Railway wagon";
    public const string ShipRoadVehicle = "Ship Road vehicle";

    public static bool IsAeroplane(string? status) => Equals(Aeroplane, status);

    public static bool IsRoadVehicle(string? status) => Equals(RoadVehicle, status);

    public static bool IsRailwayWagon(string? status) => Equals(RailwayWagon, status);

    public static bool IsShip(string? status) => Equals(Ship, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    public static bool IsRoadVehicleAeroplane(string? status) => Equals(RoadVehicleAeroplane, status);

    public static bool IsShipRailwayWagon(string? status) => Equals(ShipRailwayWagon, status);

    public static bool IsShipRoadVehicle(string? status) => Equals(ShipRoadVehicle, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

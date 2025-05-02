namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DetailsOnReExportTransportType
{
    public const string Rail = "rail";
    public const string Plane = "plane";
    public const string Ship = "ship";
    public const string Road = "road";
    public const string Other = "other";
    public const string CShipRoad = "c_ship_road";
    public const string CShipRail = "c_ship_rail";

    public static bool IsRail(string? status) => Equals(Rail, status);

    public static bool IsPlane(string? status) => Equals(Plane, status);

    public static bool IsShip(string? status) => Equals(Ship, status);

    public static bool IsRoad(string? status) => Equals(Road, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    public static bool IsCShipRoad(string? status) => Equals(CShipRoad, status);

    public static bool IsCShipRail(string? status) => Equals(CShipRail, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

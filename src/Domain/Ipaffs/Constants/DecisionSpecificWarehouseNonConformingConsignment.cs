namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionSpecificWarehouseNonConformingConsignment
{
    public const string Customwarehouse = "CustomWarehouse";
    public const string Freezoneorfreewarehouse = "FreeZoneOrFreeWarehouse";
    public const string Shipsupplier = "ShipSupplier";
    public const string Ship = "Ship";

    public static bool IsCustomwarehouse(string? status) => Equals(Customwarehouse, status);

    public static bool IsFreezoneorfreewarehouse(string? status) => Equals(Freezoneorfreewarehouse, status);

    public static bool IsShipsupplier(string? status) => Equals(Shipsupplier, status);

    public static bool IsShip(string? status) => Equals(Ship, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

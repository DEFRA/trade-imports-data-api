namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionSpecificWarehouseNonConformingConsignment
{
    public const string CustomWarehouse = "CustomWarehouse";
    public const string FreeZoneOrFreeWarehouse = "FreeZoneOrFreeWarehouse";
    public const string ShipSupplier = "ShipSupplier";
    public const string Ship = "Ship";

    public static bool IsCustomWarehouse(string? status) => Equals(CustomWarehouse, status);

    public static bool IsFreeZoneOrFreeWarehouse(string? status) => Equals(FreeZoneOrFreeWarehouse, status);

    public static bool IsShipSupplier(string? status) => Equals(ShipSupplier, status);

    public static bool IsShip(string? status) => Equals(Ship, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class PurposeForNonConforming
{
    public const string CustomsWarehouse = "Customs Warehouse";
    public const string FreeZoneOrFreeWarehouse = "Free Zone or Free Warehouse";
    public const string ShipSupplier = "Ship Supplier";
    public const string Ship = "Ship";

    public static bool IsCustomsWarehouse(string? status) => Equals(CustomsWarehouse, status);

    public static bool IsFreeZoneOrFreeWarehouse(string? status) => Equals(FreeZoneOrFreeWarehouse, status);

    public static bool IsShipSupplier(string? status) => Equals(ShipSupplier, status);

    public static bool IsShip(string? status) => Equals(Ship, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}

using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionSpecificWarehouseNonConformingConsignmentTests
{
    [Theory]
    [InlineData("CustomWarehouse", true)]
    [InlineData("customwarehouse", true)]
    [InlineData(null, false)]
    public void WhenCustomWarehouse_ThenMatch(string? status, bool expected)
    {
        DecisionSpecificWarehouseNonConformingConsignment.IsCustomWarehouse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("FreeZoneOrFreeWarehouse", true)]
    [InlineData("freezoneorfreewarehouse", true)]
    [InlineData(null, false)]
    public void WhenFreeZoneOrFreeWarehouse_ThenMatch(string? status, bool expected)
    {
        DecisionSpecificWarehouseNonConformingConsignment.IsFreeZoneOrFreeWarehouse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("ShipSupplier", true)]
    [InlineData("shipsupplier", true)]
    [InlineData(null, false)]
    public void WhenShipSupplier_ThenMatch(string? status, bool expected)
    {
        DecisionSpecificWarehouseNonConformingConsignment.IsShipSupplier(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Ship", true)]
    [InlineData("ship", true)]
    [InlineData(null, false)]
    public void WhenShip_ThenMatch(string? status, bool expected)
    {
        DecisionSpecificWarehouseNonConformingConsignment.IsShip(status).Should().Be(expected);
    }
}

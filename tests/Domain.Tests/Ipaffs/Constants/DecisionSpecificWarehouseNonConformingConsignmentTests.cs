using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionSpecificWarehouseNonConformingConsignmentTests
{
    [Theory]
    [InlineData("CustomWarehouse", true)]
    [InlineData("customwarehouse", true)]
    [InlineData(null, false)]
    public void WhenCustomwarehouse_ThenMatch(string? status, bool expected)
    {
        DecisionSpecificWarehouseNonConformingConsignment.IsCustomwarehouse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("FreeZoneOrFreeWarehouse", true)]
    [InlineData("freezoneorfreewarehouse", true)]
    [InlineData(null, false)]
    public void WhenFreezoneorfreewarehouse_ThenMatch(string? status, bool expected)
    {
        DecisionSpecificWarehouseNonConformingConsignment.IsFreezoneorfreewarehouse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("ShipSupplier", true)]
    [InlineData("shipsupplier", true)]
    [InlineData(null, false)]
    public void WhenShipsupplier_ThenMatch(string? status, bool expected)
    {
        DecisionSpecificWarehouseNonConformingConsignment.IsShipsupplier(status).Should().Be(expected);
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

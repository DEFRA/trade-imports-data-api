using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PurposeForNonConformingTests
{
    [Theory]
    [InlineData("Customs Warehouse", true)]
    [InlineData("customs warehouse", true)]
    [InlineData(null, false)]
    public void WhenCustomsWarehouse_ThenMatch(string? status, bool expected)
    {
        PurposeForNonConforming.IsCustomsWarehouse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Free Zone or Free Warehouse", true)]
    [InlineData("free zone or free warehouse", true)]
    [InlineData(null, false)]
    public void WhenFreeZoneOrFreeWarehouse_ThenMatch(string? status, bool expected)
    {
        PurposeForNonConforming.IsFreeZoneOrFreeWarehouse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Ship Supplier", true)]
    [InlineData("ship supplier", true)]
    [InlineData(null, false)]
    public void WhenShipSupplier_ThenMatch(string? status, bool expected)
    {
        PurposeForNonConforming.IsShipSupplier(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Ship", true)]
    [InlineData("ship", true)]
    [InlineData(null, false)]
    public void WhenShip_ThenMatch(string? status, bool expected)
    {
        PurposeForNonConforming.IsShip(status).Should().Be(expected);
    }
}

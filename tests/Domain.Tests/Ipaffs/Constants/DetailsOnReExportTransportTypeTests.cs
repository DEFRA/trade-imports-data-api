using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DetailsOnReExportTransportTypeTests
{
    [Theory]
    [InlineData("rail", true)]
    [InlineData("RAIL", true)]
    [InlineData(null, false)]
    public void WhenRail_ThenMatch(string? status, bool expected)
    {
        DetailsOnReExportTransportType.IsRail(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("plane", true)]
    [InlineData("PLANE", true)]
    [InlineData(null, false)]
    public void WhenPlane_ThenMatch(string? status, bool expected)
    {
        DetailsOnReExportTransportType.IsPlane(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("ship", true)]
    [InlineData("SHIP", true)]
    [InlineData(null, false)]
    public void WhenShip_ThenMatch(string? status, bool expected)
    {
        DetailsOnReExportTransportType.IsShip(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("road", true)]
    [InlineData("ROAD", true)]
    [InlineData(null, false)]
    public void WhenRoad_ThenMatch(string? status, bool expected)
    {
        DetailsOnReExportTransportType.IsRoad(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("other", true)]
    [InlineData("OTHER", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        DetailsOnReExportTransportType.IsOther(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("c_ship_road", true)]
    [InlineData("C_SHIP_ROAD", true)]
    [InlineData(null, false)]
    public void WhenCShipRoad_ThenMatch(string? status, bool expected)
    {
        DetailsOnReExportTransportType.IsCShipRoad(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("c_ship_rail", true)]
    [InlineData("C_SHIP_RAIL", true)]
    [InlineData(null, false)]
    public void WhenCShipRail_ThenMatch(string? status, bool expected)
    {
        DetailsOnReExportTransportType.IsCShipRail(status).Should().Be(expected);
    }
}

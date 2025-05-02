using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class MeansOfTransportTypeTests
{
    [Theory]
    [InlineData("Aeroplane", true)]
    [InlineData("aeroplane", true)]
    [InlineData(null, false)]
    public void WhenAeroplane_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsAeroplane(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Road Vehicle", true)]
    [InlineData("road vehicle", true)]
    [InlineData(null, false)]
    public void WhenRoadVehicle_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsRoadVehicle(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Railway Wagon", true)]
    [InlineData("railway wagon", true)]
    [InlineData(null, false)]
    public void WhenRailwayWagon_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsRailwayWagon(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Ship", true)]
    [InlineData("ship", true)]
    [InlineData(null, false)]
    public void WhenShip_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsShip(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsOther(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Road vehicle Aeroplane", true)]
    [InlineData("road vehicle aeroplane", true)]
    [InlineData(null, false)]
    public void WhenRoadVehicleAeroplane_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsRoadVehicleAeroplane(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Ship Railway wagon", true)]
    [InlineData("ship railway wagon", true)]
    [InlineData(null, false)]
    public void WhenShipRailwayWagon_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsShipRailwayWagon(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Ship Road vehicle", true)]
    [InlineData("ship road vehicle", true)]
    [InlineData(null, false)]
    public void WhenShipRoadVehicle_ThenMatch(string? status, bool expected)
    {
        MeansOfTransportType.IsShipRoadVehicle(status).Should().Be(expected);
    }
}

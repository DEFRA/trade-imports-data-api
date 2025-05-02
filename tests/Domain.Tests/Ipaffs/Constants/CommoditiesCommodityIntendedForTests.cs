using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class CommoditiesCommodityIntendedForTests
{
    [Theory]
    [InlineData("human", true)]
    [InlineData("HUMAN", true)]
    [InlineData(null, false)]
    public void WhenHuman_ThenMatch(string? status, bool expected)
    {
        CommoditiesCommodityIntendedFor.IsHuman(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("feedingstuff", true)]
    [InlineData("FEEDINGSTUFF", true)]
    [InlineData(null, false)]
    public void WhenFeedingstuff_ThenMatch(string? status, bool expected)
    {
        CommoditiesCommodityIntendedFor.IsFeedingstuff(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("further", true)]
    [InlineData("FURTHER", true)]
    [InlineData(null, false)]
    public void WhenFurther_ThenMatch(string? status, bool expected)
    {
        CommoditiesCommodityIntendedFor.IsFurther(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("other", true)]
    [InlineData("OTHER", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        CommoditiesCommodityIntendedFor.IsOther(status).Should().Be(expected);
    }
}

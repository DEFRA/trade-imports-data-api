using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PartOneTypeOfImpTests
{
    [Theory]
    [InlineData("A", true)]
    [InlineData("a", true)]
    [InlineData(null, false)]
    public void WhenA_ThenMatch(string? status, bool expected)
    {
        PartOneTypeOfImp.IsA(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("P", true)]
    [InlineData("p", true)]
    [InlineData(null, false)]
    public void WhenP_ThenMatch(string? status, bool expected)
    {
        PartOneTypeOfImp.IsP(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("D", true)]
    [InlineData("d", true)]
    [InlineData(null, false)]
    public void WhenD_ThenMatch(string? status, bool expected)
    {
        PartOneTypeOfImp.IsD(status).Should().Be(expected);
    }
}

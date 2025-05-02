using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ChedppNotAcceptableReasonCommodityOrPackageTests
{
    [Theory]
    [InlineData("c", true)]
    [InlineData("C", true)]
    [InlineData(null, false)]
    public void WhenC_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonCommodityOrPackage.IsC(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("p", true)]
    [InlineData("P", true)]
    [InlineData(null, false)]
    public void WhenP_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonCommodityOrPackage.IsP(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("cp", true)]
    [InlineData("CP", true)]
    [InlineData(null, false)]
    public void WhenCp_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonCommodityOrPackage.IsCp(status).Should().Be(expected);
    }
}

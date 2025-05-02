using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ChedppNotAcceptableReasonsCommodityOrPackageTests
{
    [Theory]
    [InlineData("c", true)]
    [InlineData("C", true)]
    [InlineData(null, false)]
    public void WhenC_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonsCommodityOrPackage.IsC(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("p", true)]
    [InlineData("P", true)]
    [InlineData(null, false)]
    public void WhenP_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonsCommodityOrPackage.IsP(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("cp", true)]
    [InlineData("CP", true)]
    [InlineData(null, false)]
    public void WhenCp_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonsCommodityOrPackage.IsCp(status).Should().Be(expected);
    }
}

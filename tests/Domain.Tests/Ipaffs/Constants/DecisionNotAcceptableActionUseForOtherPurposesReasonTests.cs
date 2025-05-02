using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionNotAcceptableActionUseForOtherPurposesReasonTests
{
    [Theory]
    [InlineData("ContaminatedProducts", true)]
    [InlineData("contaminatedproducts", true)]
    [InlineData(null, false)]
    public void WhenContaminatedproducts_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionUseForOtherPurposesReason.IsContaminatedproducts(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("InterceptedPart", true)]
    [InlineData("interceptedpart", true)]
    [InlineData(null, false)]
    public void WhenInterceptedpart_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionUseForOtherPurposesReason.IsInterceptedpart(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("PackagingMaterial", true)]
    [InlineData("packagingmaterial", true)]
    [InlineData(null, false)]
    public void WhenPackagingmaterial_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionUseForOtherPurposesReason.IsPackagingmaterial(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("MeansOfTransport", true)]
    [InlineData("meansoftransport", true)]
    [InlineData(null, false)]
    public void WhenMeansoftransport_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionUseForOtherPurposesReason.IsMeansoftransport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionUseForOtherPurposesReason.IsOther(status).Should().Be(expected);
    }
}

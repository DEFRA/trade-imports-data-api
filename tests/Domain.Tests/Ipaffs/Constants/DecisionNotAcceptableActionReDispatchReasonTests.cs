using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionNotAcceptableActionReDispatchReasonTests
{
    [Theory]
    [InlineData("ContaminatedProducts", true)]
    [InlineData("contaminatedproducts", true)]
    [InlineData(null, false)]
    public void WhenContaminatedProducts_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionReDispatchReason.IsContaminatedProducts(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("InterceptedPart", true)]
    [InlineData("interceptedpart", true)]
    [InlineData(null, false)]
    public void WhenInterceptedPart_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionReDispatchReason.IsInterceptedPart(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("PackagingMaterial", true)]
    [InlineData("packagingmaterial", true)]
    [InlineData(null, false)]
    public void WhenPackagingMaterial_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionReDispatchReason.IsPackagingMaterial(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("MeansOfTransport", true)]
    [InlineData("meansoftransport", true)]
    [InlineData(null, false)]
    public void WhenMeansOfTransport_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionReDispatchReason.IsMeansOfTransport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionReDispatchReason.IsOther(status).Should().Be(expected);
    }
}

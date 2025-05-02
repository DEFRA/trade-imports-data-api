using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionNotAcceptableActionEntryRefusalReasonTests
{
    [Theory]
    [InlineData("ContaminatedProducts", true)]
    [InlineData("contaminatedproducts", true)]
    [InlineData(null, false)]
    public void WhenContaminatedProducts_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionEntryRefusalReason.IsContaminatedProducts(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("InterceptedPart", true)]
    [InlineData("interceptedpart", true)]
    [InlineData(null, false)]
    public void WhenInterceptedPart_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionEntryRefusalReason.IsInterceptedPart(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("PackagingMaterial", true)]
    [InlineData("packagingmaterial", true)]
    [InlineData(null, false)]
    public void WhenPackagingMaterial_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionEntryRefusalReason.IsPackagingMaterial(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("MeansOfTransport", true)]
    [InlineData("meansoftransport", true)]
    [InlineData(null, false)]
    public void WhenMeansOfTransport_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionEntryRefusalReason.IsMeansOfTransport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionEntryRefusalReason.IsOther(status).Should().Be(expected);
    }
}

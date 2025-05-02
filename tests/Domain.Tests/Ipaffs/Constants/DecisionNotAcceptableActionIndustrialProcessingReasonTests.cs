using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionNotAcceptableActionIndustrialProcessingReasonTests
{
    [Theory]
    [InlineData("ContaminatedProducts", true)]
    [InlineData("contaminatedproducts", true)]
    [InlineData(null, false)]
    public void WhenContaminatedProducts_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionIndustrialProcessingReason.IsContaminatedProducts(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("InterceptedPart", true)]
    [InlineData("interceptedpart", true)]
    [InlineData(null, false)]
    public void WhenInterceptedPart_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionIndustrialProcessingReason.IsInterceptedPart(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("PackagingMaterial", true)]
    [InlineData("packagingmaterial", true)]
    [InlineData(null, false)]
    public void WhenPackagingMaterial_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionIndustrialProcessingReason.IsPackagingMaterial(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionIndustrialProcessingReason.IsOther(status).Should().Be(expected);
    }
}

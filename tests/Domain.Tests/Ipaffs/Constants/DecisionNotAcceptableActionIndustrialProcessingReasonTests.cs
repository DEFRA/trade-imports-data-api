using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionNotAcceptableActionIndustrialProcessingReasonTests
{
    [Theory]
    [InlineData("ContaminatedProducts", true)]
    [InlineData("contaminatedproducts", true)]
    [InlineData(null, false)]
    public void WhenContaminatedproducts_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionIndustrialProcessingReason.IsContaminatedproducts(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("InterceptedPart", true)]
    [InlineData("interceptedpart", true)]
    [InlineData(null, false)]
    public void WhenInterceptedpart_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionIndustrialProcessingReason.IsInterceptedpart(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("PackagingMaterial", true)]
    [InlineData("packagingmaterial", true)]
    [InlineData(null, false)]
    public void WhenPackagingmaterial_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableActionIndustrialProcessingReason.IsPackagingmaterial(status).Should().Be(expected);
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

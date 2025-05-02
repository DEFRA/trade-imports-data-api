using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class CommodityResultsPhsiClassificationTests
{
    [Theory]
    [InlineData("Mandatory", true)]
    [InlineData("mandatory", true)]
    [InlineData(null, false)]
    public void WhenMandatory_ThenMatch(string? status, bool expected)
    {
        CommodityResultsPhsiClassification.IsMandatory(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Reduced", true)]
    [InlineData("reduced", true)]
    [InlineData(null, false)]
    public void WhenReduced_ThenMatch(string? status, bool expected)
    {
        CommodityResultsPhsiClassification.IsReduced(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Controlled", true)]
    [InlineData("controlled", true)]
    [InlineData(null, false)]
    public void WhenControlled_ThenMatch(string? status, bool expected)
    {
        CommodityResultsPhsiClassification.IsControlled(status).Should().Be(expected);
    }
}

using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PartOneProvideCtcMrnTests
{
    [Theory]
    [InlineData("YES", true)]
    [InlineData("yes", true)]
    [InlineData(null, false)]
    public void WhenYes_ThenMatch(string? status, bool expected)
    {
        PartOneProvideCtcMrn.IsYes(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("YES_ADD_LATER", true)]
    [InlineData("yes_add_later", true)]
    [InlineData(null, false)]
    public void WhenYesAddLater_ThenMatch(string? status, bool expected)
    {
        PartOneProvideCtcMrn.IsYesAddLater(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("NO", true)]
    [InlineData("no", true)]
    [InlineData(null, false)]
    public void WhenNo_ThenMatch(string? status, bool expected)
    {
        PartOneProvideCtcMrn.IsNo(status).Should().Be(expected);
    }
}

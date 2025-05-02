using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PartThreeControlStatusTests
{
    [Theory]
    [InlineData("REQUIRED", true)]
    [InlineData("required", true)]
    [InlineData(null, false)]
    public void WhenRequired_ThenMatch(string? status, bool expected)
    {
        PartThreeControlStatus.IsRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("COMPLETED", true)]
    [InlineData("completed", true)]
    [InlineData(null, false)]
    public void WhenCompleted_ThenMatch(string? status, bool expected)
    {
        PartThreeControlStatus.IsCompleted(status).Should().Be(expected);
    }
}

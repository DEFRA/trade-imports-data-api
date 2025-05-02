using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionFreeCirculationPurposeTests
{
    [Theory]
    [InlineData("Animal Feeding Stuff", true)]
    [InlineData("animal feeding stuff", true)]
    [InlineData(null, false)]
    public void WhenAnimalFeedingStuff_ThenMatch(string? status, bool expected)
    {
        DecisionFreeCirculationPurpose.IsAnimalFeedingStuff(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Human Consumption", true)]
    [InlineData("human consumption", true)]
    [InlineData(null, false)]
    public void WhenHumanConsumption_ThenMatch(string? status, bool expected)
    {
        DecisionFreeCirculationPurpose.IsHumanConsumption(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Pharmaceutical Use", true)]
    [InlineData("pharmaceutical use", true)]
    [InlineData(null, false)]
    public void WhenPharmaceuticalUse_ThenMatch(string? status, bool expected)
    {
        DecisionFreeCirculationPurpose.IsPharmaceuticalUse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Technical Use", true)]
    [InlineData("technical use", true)]
    [InlineData(null, false)]
    public void WhenTechnicalUse_ThenMatch(string? status, bool expected)
    {
        DecisionFreeCirculationPurpose.IsTechnicalUse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Further Process", true)]
    [InlineData("further process", true)]
    [InlineData(null, false)]
    public void WhenFurtherProcess_ThenMatch(string? status, bool expected)
    {
        DecisionFreeCirculationPurpose.IsFurtherProcess(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        DecisionFreeCirculationPurpose.IsOther(status).Should().Be(expected);
    }
}

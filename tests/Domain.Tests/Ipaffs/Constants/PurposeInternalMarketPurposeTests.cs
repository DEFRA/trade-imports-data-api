using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PurposeInternalMarketPurposeTests
{
    [Theory]
    [InlineData("Animal Feeding Stuff", true)]
    [InlineData("animal feeding stuff", true)]
    [InlineData(null, false)]
    public void WhenAnimalFeedingStuff_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsAnimalFeedingStuff(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Human Consumption", true)]
    [InlineData("human consumption", true)]
    [InlineData(null, false)]
    public void WhenHumanConsumption_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsHumanConsumption(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Pharmaceutical Use", true)]
    [InlineData("pharmaceutical use", true)]
    [InlineData(null, false)]
    public void WhenPharmaceuticalUse_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsPharmaceuticalUse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Technical Use", true)]
    [InlineData("technical use", true)]
    [InlineData(null, false)]
    public void WhenTechnicalUse_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsTechnicalUse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsOther(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Commercial Sale", true)]
    [InlineData("commercial sale", true)]
    [InlineData(null, false)]
    public void WhenCommercialSale_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsCommercialSale(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Commercial sale or change of ownership", true)]
    [InlineData("commercial sale or change of ownership", true)]
    [InlineData(null, false)]
    public void WhenCommercialSaleOrChangeOfOwnership_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsCommercialSaleOrChangeOfOwnership(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Rescue", true)]
    [InlineData("rescue", true)]
    [InlineData(null, false)]
    public void WhenRescue_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsRescue(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Breeding", true)]
    [InlineData("breeding", true)]
    [InlineData(null, false)]
    public void WhenBreeding_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsBreeding(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Research", true)]
    [InlineData("research", true)]
    [InlineData(null, false)]
    public void WhenResearch_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsResearch(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Racing or Competition", true)]
    [InlineData("racing or competition", true)]
    [InlineData(null, false)]
    public void WhenRacingOrCompetition_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsRacingOrCompetition(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Approved Premises or Body", true)]
    [InlineData("approved premises or body", true)]
    [InlineData(null, false)]
    public void WhenApprovedPremisesOrBody_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsApprovedPremisesOrBody(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Companion Animal not for Resale or Rehoming", true)]
    [InlineData("companion animal not for resale or rehoming", true)]
    [InlineData(null, false)]
    public void WhenCompanionAnimalNotForResaleOrRehoming_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsCompanionAnimalNotForResaleOrRehoming(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Production", true)]
    [InlineData("production", true)]
    [InlineData(null, false)]
    public void WhenProduction_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsProduction(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Slaughter", true)]
    [InlineData("slaughter", true)]
    [InlineData(null, false)]
    public void WhenSlaughter_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsSlaughter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Fattening", true)]
    [InlineData("fattening", true)]
    [InlineData(null, false)]
    public void WhenFattening_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsFattening(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Game Restocking", true)]
    [InlineData("game restocking", true)]
    [InlineData(null, false)]
    public void WhenGameRestocking_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsGameRestocking(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Registered Horses", true)]
    [InlineData("registered horses", true)]
    [InlineData(null, false)]
    public void WhenRegisteredHorses_ThenMatch(string? status, bool expected)
    {
        PurposeInternalMarketPurpose.IsRegisteredHorses(status).Should().Be(expected);
    }
}

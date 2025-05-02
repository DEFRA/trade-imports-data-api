using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class CommoditiesAnimalsCertifiedAsTests
{
    [Theory]
    [InlineData("Animal feeding stuff", true)]
    [InlineData("animal feeding stuff", true)]
    [InlineData(null, false)]
    public void WhenAnimalFeedingStuff_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsAnimalFeedingStuff(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Approved", true)]
    [InlineData("approved", true)]
    [InlineData(null, false)]
    public void WhenApproved_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsApproved(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Artificial reproduction", true)]
    [InlineData("artificial reproduction", true)]
    [InlineData(null, false)]
    public void WhenArtificialReproduction_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsArtificialReproduction(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Breeding", true)]
    [InlineData("breeding", true)]
    [InlineData(null, false)]
    public void WhenBreeding_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsBreeding(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Circus", true)]
    [InlineData("circus", true)]
    [InlineData(null, false)]
    public void WhenCircus_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsCircus(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Commercial sale", true)]
    [InlineData("commercial sale", true)]
    [InlineData(null, false)]
    public void WhenCommercialSale_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsCommercialSale(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Commercial sale or change of ownership", true)]
    [InlineData("commercial sale or change of ownership", true)]
    [InlineData(null, false)]
    public void WhenCommercialSaleOrChangeOfOwnership_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsCommercialSaleOrChangeOfOwnership(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Fattening", true)]
    [InlineData("fattening", true)]
    [InlineData(null, false)]
    public void WhenFattening_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsFattening(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Game restocking", true)]
    [InlineData("game restocking", true)]
    [InlineData(null, false)]
    public void WhenGameRestocking_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsGameRestocking(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Human consumption", true)]
    [InlineData("human consumption", true)]
    [InlineData(null, false)]
    public void WhenHumanConsumption_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsHumanConsumption(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Internal market", true)]
    [InlineData("internal market", true)]
    [InlineData(null, false)]
    public void WhenInternalMarket_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsInternalMarket(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Other", true)]
    [InlineData("other", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsOther(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Personally owned pets not for rehoming", true)]
    [InlineData("personally owned pets not for rehoming", true)]
    [InlineData(null, false)]
    public void WhenPersonallyOwnedPetsNotForRehoming_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsPersonallyOwnedPetsNotForRehoming(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Pets", true)]
    [InlineData("pets", true)]
    [InlineData(null, false)]
    public void WhenPets_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsPets(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Production", true)]
    [InlineData("production", true)]
    [InlineData(null, false)]
    public void WhenProduction_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsProduction(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Quarantine", true)]
    [InlineData("quarantine", true)]
    [InlineData(null, false)]
    public void WhenQuarantine_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsQuarantine(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Racing/Competition", true)]
    [InlineData("racing/competition", true)]
    [InlineData(null, false)]
    public void WhenRacingCompetition_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsRacingCompetition(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Registered equidae", true)]
    [InlineData("registered equidae", true)]
    [InlineData(null, false)]
    public void WhenRegisteredEquidae_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsRegisteredEquidae(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Registered", true)]
    [InlineData("registered", true)]
    [InlineData(null, false)]
    public void WhenRegistered_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsRegistered(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Rejected or Returned consignment", true)]
    [InlineData("rejected or returned consignment", true)]
    [InlineData(null, false)]
    public void WhenRejectedOrReturnedConsignment_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsRejectedOrReturnedConsignment(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Relaying", true)]
    [InlineData("relaying", true)]
    [InlineData(null, false)]
    public void WhenRelaying_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsRelaying(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Rescue/Rehoming", true)]
    [InlineData("rescue/rehoming", true)]
    [InlineData(null, false)]
    public void WhenRescueRehoming_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsRescueRehoming(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Research", true)]
    [InlineData("research", true)]
    [InlineData(null, false)]
    public void WhenResearch_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsResearch(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Slaughter", true)]
    [InlineData("slaughter", true)]
    [InlineData(null, false)]
    public void WhenSlaughter_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsSlaughter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Technical/Pharmaceutical use", true)]
    [InlineData("technical/pharmaceutical use", true)]
    [InlineData(null, false)]
    public void WhenTechnicalPharmaceuticalUse_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsTechnicalPharmaceuticalUse(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Transit", true)]
    [InlineData("transit", true)]
    [InlineData(null, false)]
    public void WhenTransit_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsTransit(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Zoo/Collection", true)]
    [InlineData("zoo/collection", true)]
    [InlineData(null, false)]
    public void WhenZooCollection_ThenMatch(string? status, bool expected)
    {
        CommoditiesAnimalsCertifiedAs.IsZooCollection(status).Should().Be(expected);
    }
}

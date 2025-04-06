using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Defra.TradeImportsDataApi.Domain.MongoDB.Tests;

public class BsonDateTimeSerializationTests
{
    [Fact]
    public void DomainTypesWithUnspecifiedDateTimeKind_RoundTripAsExpected()
    {
        ClassMapConfiguration.Register();

        var dateTime = new DateTime(2025, 4, 6, 18, 0, 0, DateTimeKind.Unspecified);

        ActAndAssertRoundTrip(new PlannedCrossing { DepartsAt = dateTime }, x => x.DepartsAt);
        ActAndAssertRoundTrip(new ActualCrossing { ArrivesAt = dateTime }, x => x.ArrivesAt);
        ActAndAssertRoundTrip(new Applicant { SampledOn = dateTime }, x => x.SampledOn);
        ActAndAssertRoundTrip(
            new JourneyRiskCategorisationResult { RiskLevelSetFor = dateTime },
            x => x.RiskLevelSetFor
        );
        ActAndAssertRoundTrip(new LaboratoryTests { TestedOn = dateTime }, x => x.TestedOn);
        ActAndAssertRoundTrip(new PartOne { ExitedPortOfOn = dateTime }, x => x.ExitedPortOfOn);
        ActAndAssertRoundTrip(new PartOne { ArrivesAt = dateTime }, x => x.ArrivesAt);
        ActAndAssertRoundTrip(new PartOne { DepartedOn = dateTime }, x => x.DepartedOn);
        ActAndAssertRoundTrip(new RiskAssessmentResult { AssessedOn = dateTime }, x => x.AssessedOn);
        ActAndAssertRoundTrip(new ActualCrossing { ArrivesAt = dateTime }, x => x.ArrivesAt);
        ActAndAssertRoundTrip(new CheckedInCrossing { ArrivesAt = dateTime }, x => x.ArrivesAt);
        ActAndAssertRoundTrip(new SealCheck { CheckedOn = dateTime }, x => x.CheckedOn);
    }

    private static void ActAndAssertRoundTrip<T>(T subject, Func<T, DateTime?> resolveDateTime)
    {
        var bsonDocument = subject.ToBsonDocument();

        bsonDocument.Should().NotBeNull();

        var subject2 = BsonSerializer.Deserialize<T>(bsonDocument);

        subject2.Should().NotBeNull();

        var dateTimeValue = resolveDateTime(subject2);
        dateTimeValue.Should().NotBeNull();
        dateTimeValue.Should().Be(resolveDateTime(subject), "(type tested was {0})", typeof(T).Name);
        dateTimeValue.Value.Kind.Should().Be(DateTimeKind.Unspecified);
    }
}

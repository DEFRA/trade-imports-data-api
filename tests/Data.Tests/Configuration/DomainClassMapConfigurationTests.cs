using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Defra.TradeImportsDataApi.Data.Tests.Configuration;

public class DomainClassMapConfigurationTests
{
    /// <summary>
    /// Given an Unspecified DateTime Kind, serialize to BSON and then round trip back to the original type,
    /// ensuring the Unspecified DateTime Kind is not lost.
    /// </summary>
    [Fact]
    public void DomainTypesWithUnspecifiedDateTimeKind_RoundTripAsExpected()
    {
        DomainClassMapConfiguration.Register();

        var dateTime = new DateTime(2025, 4, 6, 18, 0, 0, DateTimeKind.Unspecified);

        // Gvms
        SerializeThenDeserializeAndAssertKind(new PlannedCrossing { DepartsAt = dateTime }, x => x.DepartsAt);
        SerializeThenDeserializeAndAssertKind(new ActualCrossing { ArrivesAt = dateTime }, x => x.ArrivesAt);
        SerializeThenDeserializeAndAssertKind(new CheckedInCrossing { ArrivesAt = dateTime }, x => x.ArrivesAt);

        // Ipaffs
        SerializeThenDeserializeAndAssertKind(new Applicant { SampledOn = dateTime }, x => x.SampledOn);
        SerializeThenDeserializeAndAssertKind(
            new JourneyRiskCategorisationResult { RiskLevelSetFor = dateTime },
            x => x.RiskLevelSetFor
        );
        SerializeThenDeserializeAndAssertKind(new LaboratoryTests { TestedOn = dateTime }, x => x.TestedOn);
        SerializeThenDeserializeAndAssertKind(new PartOne { ExitedPortOfOn = dateTime }, x => x.ExitedPortOfOn);
        SerializeThenDeserializeAndAssertKind(new PartOne { ArrivesAt = dateTime }, x => x.ArrivesAt);
        SerializeThenDeserializeAndAssertKind(new PartOne { DepartedOn = dateTime }, x => x.DepartedOn);
        SerializeThenDeserializeAndAssertKind(new RiskAssessmentResult { AssessedOn = dateTime }, x => x.AssessedOn);
        SerializeThenDeserializeAndAssertKind(new SealCheck { CheckedOn = dateTime }, x => x.CheckedOn);
    }

    private static void SerializeThenDeserializeAndAssertKind<T>(T subject, Func<T, DateTime?> resolveDateTime)
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

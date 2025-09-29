using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class DataEntityExtensionsTests
{
    [Fact]
    public void WhenToResourceEvent_ShouldMap()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

        result.ResourceId.Should().Be("id");
        result.ResourceType.Should().Be("Fixture");
        result.Operation.Should().Be(ResourceEventOperations.Created);
        result.ETag.Should().Be("etag");
        result.Resource.Should().Be(subject);
    }

    [Fact]
    public void WhenToResourceEvent_AndOperationIsUnknown_ShouldThrow()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

        var act = () => subject.ToResourceEvent("unknown", new FixtureDomain(), new FixtureDomain());

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Operation must be either Updated or Created (Parameter 'operation')");
    }

    [Theory]
    [InlineData(ResourceEventOperations.Created, 0)]
    [InlineData(ResourceEventOperations.Updated, 1)]
    public void WhenToResourceEvent_AndOperation_ChangeSetShouldBeExpectedCount(string operation, int expectedCount)
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

        var result = subject.ToResourceEvent(operation, new FixtureDomain { Id = 1 }, new FixtureDomain());

        result.ChangeSet.Count.Should().Be(expectedCount);
    }

    [Fact]
    public void WhenToResourceEvent_AndEntityShouldNotBeIncludedAsResource_ResourceShouldBeNull()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

        var result = subject.ToResourceEvent(
            ResourceEventOperations.Created,
            new FixtureDomain(),
            new FixtureDomain(),
            includeEntityAsResource: false
        );

        result.Resource.Should().BeNull();
    }

    [Fact]
    public void WhenToResourceEvent_AndEntityIsImportPreNotification_ShouldMap()
    {
        var subject = new ImportPreNotificationEntity
        {
            Id = "id",
            ImportPreNotification = new ImportPreNotification(),
        };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

        result.ResourceType.Should().Be("ImportPreNotification");
    }

    [Fact]
    public void WhenToResourceEvent_AndEntityIsCustomsDeclaration_ShouldMap()
    {
        var subject = new CustomsDeclarationEntity { Id = "id" };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

        result.ResourceType.Should().Be("CustomsDeclaration");
    }

    [Fact]
    public void WhenToResourceEvent_AndEntityIsProcessingError_ShouldMap()
    {
        var subject = new ProcessingErrorEntity { Id = "id", ProcessingErrors = [] };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

        result.ResourceType.Should().Be("ProcessingError");
    }

    [Fact]
    public void WhenToResourceEvent_AndMultipleUnknownSubFieldsChanging_ShouldNotThrow()
    {
        var previous = new FixtureEntity
        {
            Name = "From",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value1,
        };
        var current = new FixtureEntity
        {
            Name = "To",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value2,
        };

        var act = () => current.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        act.Should().NotThrow<InvalidOperationException>();
    }

    [Fact]
    public void WhenToResourceEvent_ShouldCreateChangeSet()
    {
        var previous = new FixtureEntity
        {
            Name = "From",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value1,
        };
        var current = new FixtureEntity
        {
            Name = "To",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value2,
        };
        var result = current.ToResourceEvent(ResourceEventOperations.Updated, current, previous);

        result.ChangeSet.Count.Should().Be(2);
        result.ChangeSet[0].Operation.Should().Be("Replace");
        result.ChangeSet[0].Path.Should().Be("/Name");
        result.ChangeSet[0].Value.Should().Be("To");
        result.ChangeSet[1].Operation.Should().Be("Replace");
        result.ChangeSet[1].Path.Should().Be("/FixtureType");
        result.ChangeSet[1].Value.Should().Be("Value2");
    }

    [Fact]
    public void WhenToResourceEvent_AndSubResourceTypeIsUnknown_ShouldNotSetSubResourceType()
    {
        var previous = new FixtureEntity
        {
            Name = "From",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value1,
        };
        var current = new FixtureEntity
        {
            Name = "To",
            Id = "id",
            ETag = "etag",
            FixtureType = FixtureType.Value1,
        };
        var result = current.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        result.SubResourceType.Should().BeNull();
    }

    [Fact]
    public void WhenToResourceEvent_AndSubResourceTypeIsClearanceRequest_ShouldSetSubResourceType()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
        var previous = new CustomsDeclaration();
        var current = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        result.SubResourceType.Should().Be("ClearanceRequest");
    }

    [Fact]
    public void WhenToResourceEvent_AndSubResourceTypeIsClearanceRequest_WithChildPropertyChange_ShouldSetSubResourceType()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
        var previous = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 1 } };
        var current = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 } };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        result.SubResourceType.Should().Be("ClearanceRequest");
    }

    [Fact]
    public void WhenToResourceEvent_AndSubResourceTypeIsClearanceDecision_ShouldSetSubResourceType()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
        var previous = new CustomsDeclaration();
        var current = new CustomsDeclaration { ClearanceDecision = new ClearanceDecision { Items = [] } };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        result.SubResourceType.Should().Be("ClearanceDecision");
    }

    [Fact]
    public void WhenToResourceEvent_AndSubResourceTypeIsFinalisation_ShouldSetSubResourceType()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
        var previous = new CustomsDeclaration();
        var current = new CustomsDeclaration
        {
            Finalisation = new Finalisation
            {
                ExternalVersion = 0,
                FinalState = "0",
                IsManualRelease = false,
            },
        };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        result.SubResourceType.Should().Be("Finalisation");
    }

    [Fact]
    public void WhenToResourceEvent_AndSubResourceTypeIsExternalError_ShouldSetSubResourceType()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
        var previous = new CustomsDeclaration();
        var current = new CustomsDeclaration { ExternalErrors = [new ExternalError()] };

        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        result.SubResourceType.Should().Be("ExternalError");
    }

    [Fact]
    public void WhenToResourceEvent_AndMultipleKnownSubResourceTypes_ShouldThrow()
    {
        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
        var previous = new CustomsDeclaration();
        var current = new CustomsDeclaration
        {
            ClearanceRequest = new ClearanceRequest(),
            ClearanceDecision = new ClearanceDecision { Items = [] },
            Finalisation = new Finalisation
            {
                ExternalVersion = 0,
                FinalState = "0",
                IsManualRelease = false,
            },
            ExternalErrors = [],
        };

        var act = () => subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

        act.Should().Throw<InvalidOperationException>();
    }

    private class FixtureEntity : IDataEntity
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string ETag { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public FixtureType FixtureType { get; set; }

        public void OnSave() { }
    }

    private enum FixtureType
    {
        Value1,
        Value2,
    }

    private class FixtureDomain
    {
        public int Id { get; set; }
    }
}

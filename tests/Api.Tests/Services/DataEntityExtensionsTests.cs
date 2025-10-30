using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class DataEntityExtensionsTests
{
    [Fact]
    public void ImportPreNotification_ToResourceEvent_ValidCreatedOperation_ReturnsExpectedResourceEvent()
    {
        // Arrange
        var entity = new ImportPreNotificationEntity
        {
            Id = "123",
            ETag = "etag123",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            ImportPreNotification = new ImportPreNotification(),
        };
        var current = new ImportPreNotification();
        var previous = new ImportPreNotification();
        // Act
        var result = entity.ToResourceEvent(
            ResourceEventOperations.Created,
            current,
            previous,
            includeEntityAsResource: true
        );
        // Assert
        Assert.NotNull(result);
        Assert.Equal("123", result.ResourceId);
        Assert.Equal(ResourceEventOperations.Created, result.Operation);
        Assert.NotNull(result.Resource);
        Assert.Empty(result.ChangeSet);
    }

    [Fact]
    public void ImportPreNotification_ToResourceEvent_InvalidOperation_ThrowsArgumentException()
    {
        // Arrange
        var entity = new ImportPreNotificationEntity
        {
            Id = "123",
            ETag = "etag123",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            ImportPreNotification = new ImportPreNotification(),
        };
        var current = new ImportPreNotification();
        var previous = new ImportPreNotification();
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            entity.ToResourceEvent("InvalidOperation", current, previous, includeEntityAsResource: true)
        );
    }

    [Fact]
    public void ImportPreNotification_ToResourceEvent_UpdatedOperation_IncludesChangeSet()
    {
        // Arrange
        var entity = new ImportPreNotificationEntity
        {
            Id = "123",
            ETag = "etag123",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            ImportPreNotification = new ImportPreNotification(),
        };
        var current = new ImportPreNotification() { ReferenceNumber = "123" };
        var previous = new ImportPreNotification() { ReferenceNumber = "321" };
        // Act
        var result = entity.ToResourceEvent(
            ResourceEventOperations.Updated,
            current,
            previous,
            includeEntityAsResource: true
        );
        // Assert
        Assert.NotNull(result);
        Assert.Equal(ResourceEventOperations.Updated, result.Operation);
        Assert.NotEmpty(result.ChangeSet);
    }

    [Fact]
    public void ProcessingError_ToResourceEvent_ShouldThrowArgumentException_ForInvalidOperation()
    {
        // Arrange
        var entity = new ProcessingErrorEntity
        {
            Id = "1",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            ProcessingErrors = Array.Empty<ProcessingError>(),
        };
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            entity.ToResourceEvent("InvalidOperation", Array.Empty<ProcessingError>(), Array.Empty<ProcessingError>())
        );
    }

    [Fact]
    public void ProcessingError_ToResourceEvent_ShouldReturnResourceEvent_ForValidOperation()
    {
        // Arrange
        var entity = new ProcessingErrorEntity
        {
            Id = "1",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            ProcessingErrors = Array.Empty<ProcessingError>(),
        };
        // Act
        var result = entity.ToResourceEvent(
            ResourceEventOperations.Created,
            Array.Empty<ProcessingError>(),
            Array.Empty<ProcessingError>()
        );
        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.ResourceId);
        Assert.Equal(ResourceEventOperations.Created, result.Operation);
        Assert.NotNull(result.Resource);
    }

    [Fact]
    public void CustomsDeclaration_ToResourceEvent_ValidInput_ReturnsExpectedResourceEvent()
    {
        // Arrange
        var entity = new CustomsDeclarationEntity
        {
            Id = "TestId",
            ClearanceDecision = null,
            ClearanceRequest = new ClearanceRequest(),
            Created = DateTime.UtcNow,
            ETag = "ETag",
            ExternalErrors = [],
            Finalisation = null,
            Updated = DateTime.UtcNow,
        };
        var current = new CustomsDeclaration();
        var previous = new CustomsDeclaration();
        string operation = ResourceEventOperations.Created;
        // Act
        var result = entity.ToResourceEvent(operation, current, previous);
        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestId", result.ResourceId);
        Assert.Equal(operation, result.Operation);
        Assert.NotNull(result.Resource);
    }

    [Fact]
    public void CustomsDeclaration_ToResourceEvent_InvalidOperation_ThrowsArgumentException()
    {
        // Arrange
        var entity = new CustomsDeclarationEntity() { Id = "123" };
        var current = new CustomsDeclaration();
        var previous = new CustomsDeclaration();
        string invalidOperation = "InvalidOperation";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => entity.ToResourceEvent(invalidOperation, current, previous));
    }
    ////[Fact]
    ////public void CustomsDeclaration_ToResourceEvent_MultipleKnownSubResourceTypes_ThrowsInvalidOperationException()

    ////{
    ////    // Arrange
    ////    // Mock or set up a changeSet with multiple known sub-resource types.
    ////    // Act & Assert
    ////    // Verify that an InvalidOperationException is thrown.
    ////}
}

////public class DataEntityExtensionsTests
////{
////    [Fact]
////    public void WhenToResourceEvent_ShouldMap()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

////        result.ResourceId.Should().Be("id");
////        result.ResourceType.Should().Be("Fixture");
////        result.Operation.Should().Be(ResourceEventOperations.Created);
////        result.ETag.Should().Be("etag");
////        result.Resource.Should().Be(subject);
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndOperationIsUnknown_ShouldThrow()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

////        var act = () => subject.ToResourceEvent("unknown", new FixtureDomain(), new FixtureDomain());

////        act.Should()
////            .Throw<ArgumentException>()
////            .WithMessage("Operation must be either Updated or Created (Parameter 'operation')");
////    }

////    [Theory]
////    [InlineData(ResourceEventOperations.Created, 0)]
////    [InlineData(ResourceEventOperations.Updated, 1)]
////    public void WhenToResourceEvent_AndOperation_ChangeSetShouldBeExpectedCount(string operation, int expectedCount)
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

////        var result = subject.ToResourceEvent(operation, new FixtureDomain { Id = 1 }, new FixtureDomain());

////        result.ChangeSet.Count.Should().Be(expectedCount);
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndEntityShouldNotBeIncludedAsResource_ResourceShouldBeNull()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };

////        var result = subject.ToResourceEvent(
////            ResourceEventOperations.Created,
////            new FixtureDomain(),
////            new FixtureDomain(),
////            includeEntityAsResource: false
////        );

////        result.Resource.Should().BeNull();
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndEntityIsImportPreNotification_ShouldMap()
////    {
////        var subject = new ImportPreNotificationEntity
////        {
////            Id = "id",
////            ImportPreNotification = new ImportPreNotification(),
////        };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

////        result.ResourceType.Should().Be("ImportPreNotification");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndEntityIsCustomsDeclaration_ShouldMap()
////    {
////        var subject = new CustomsDeclarationEntity { Id = "id" };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

////        result.ResourceType.Should().Be("CustomsDeclaration");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndEntityIsProcessingError_ShouldMap()
////    {
////        var subject = new ProcessingErrorEntity { Id = "id", ProcessingErrors = [] };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, new FixtureDomain(), new FixtureDomain());

////        result.ResourceType.Should().Be("ProcessingError");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndMultipleUnknownSubFieldsChanging_ShouldNotThrow()
////    {
////        var previous = new FixtureEntity
////        {
////            Name = "From",
////            Id = "id",
////            ETag = "etag",
////            FixtureType = FixtureType.Value1,
////        };
////        var current = new FixtureEntity
////        {
////            Name = "To",
////            Id = "id",
////            ETag = "etag",
////            FixtureType = FixtureType.Value2,
////        };

////        var act = () => current.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        act.Should().NotThrow<InvalidOperationException>();
////    }

////    [Fact]
////    public void WhenToResourceEvent_ShouldCreateChangeSet()
////    {
////        var previous = new FixtureEntity
////        {
////            Name = "From",
////            Id = "id",
////            ETag = "etag",
////            FixtureType = FixtureType.Value1,
////        };
////        var current = new FixtureEntity
////        {
////            Name = "To",
////            Id = "id",
////            ETag = "etag",
////            FixtureType = FixtureType.Value2,
////        };
////        var result = current.ToResourceEvent(ResourceEventOperations.Updated, current, previous);

////        result.ChangeSet.Count.Should().Be(2);
////        result.ChangeSet[0].Operation.Should().Be("Replace");
////        result.ChangeSet[0].Path.Should().Be("/Name");
////        result.ChangeSet[0].Value.Should().Be("To");
////        result.ChangeSet[1].Operation.Should().Be("Replace");
////        result.ChangeSet[1].Path.Should().Be("/FixtureType");
////        result.ChangeSet[1].Value.Should().Be("Value2");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndSubResourceTypeIsUnknown_ShouldNotSetSubResourceType()
////    {
////        var previous = new FixtureEntity
////        {
////            Name = "From",
////            Id = "id",
////            ETag = "etag",
////            FixtureType = FixtureType.Value1,
////        };
////        var current = new FixtureEntity
////        {
////            Name = "To",
////            Id = "id",
////            ETag = "etag",
////            FixtureType = FixtureType.Value1,
////        };
////        var result = current.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        result.SubResourceType.Should().BeNull();
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndSubResourceTypeIsClearanceRequest_ShouldSetSubResourceType()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
////        var previous = new CustomsDeclaration();
////        var current = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        result.SubResourceType.Should().Be("ClearanceRequest");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndSubResourceTypeIsClearanceRequest_WithChildPropertyChange_ShouldSetSubResourceType()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
////        var previous = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 1 } };
////        var current = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 } };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        result.SubResourceType.Should().Be("ClearanceRequest");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndSubResourceTypeIsClearanceDecision_ShouldSetSubResourceType()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
////        var previous = new CustomsDeclaration();
////        var current = new CustomsDeclaration { ClearanceDecision = new ClearanceDecision { Items = [] } };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        result.SubResourceType.Should().Be("ClearanceDecision");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndSubResourceTypeIsFinalisation_ShouldSetSubResourceType()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
////        var previous = new CustomsDeclaration();
////        var current = new CustomsDeclaration
////        {
////            Finalisation = new Finalisation
////            {
////                ExternalVersion = 0,
////                FinalState = "0",
////                IsManualRelease = false,
////            },
////        };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        result.SubResourceType.Should().Be("Finalisation");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndSubResourceTypeIsExternalError_ShouldSetSubResourceType()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
////        var previous = new CustomsDeclaration();
////        var current = new CustomsDeclaration { ExternalErrors = [new ExternalError()] };

////        var result = subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        result.SubResourceType.Should().Be("ExternalError");
////    }

////    [Fact]
////    public void WhenToResourceEvent_AndMultipleKnownSubResourceTypes_ShouldThrow()
////    {
////        var subject = new FixtureEntity { Id = "id", ETag = "etag" };
////        var previous = new CustomsDeclaration();
////        var current = new CustomsDeclaration
////        {
////            ClearanceRequest = new ClearanceRequest(),
////            ClearanceDecision = new ClearanceDecision { Items = [] },
////            Finalisation = new Finalisation
////            {
////                ExternalVersion = 0,
////                FinalState = "0",
////                IsManualRelease = false,
////            },
////            ExternalErrors = [],
////        };

////        var act = () => subject.ToResourceEvent(ResourceEventOperations.Created, current, previous);

////        act.Should().Throw<InvalidOperationException>();
////    }

////    private class FixtureEntity : IDataEntity
////    {
////        public string Name { get; set; } = null!;
////        public string Id { get; set; } = null!;
////        public string ETag { get; set; } = null!;
////        public DateTime Created { get; set; }
////        public DateTime Updated { get; set; }
////        public FixtureType FixtureType { get; set; }

////        public void OnSave() { }
////    }

////    private enum FixtureType
////    {
////        Value1,
////        Value2,
////    }

////    private class FixtureDomain
////    {
////        public int Id { get; set; }
////    }
////}

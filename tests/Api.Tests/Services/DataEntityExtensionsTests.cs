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
}

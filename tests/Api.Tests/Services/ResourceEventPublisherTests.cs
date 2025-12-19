using System.IO.Compression;
using System.Text;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Defra.TradeImportsDataApi.Api.Configuration;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using FluentAssertions;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ResourceEventPublisherTests
{
    [Fact]
    public async Task Publish_ShouldPublish()
    {
        var mockSimpleNotificationService = Substitute.For<IAmazonSimpleNotificationService>();
        var subject = new ResourceEventPublisher(
            mockSimpleNotificationService,
            new OptionsWrapper<TraceHeader>(new TraceHeader { Name = "trace-id" }),
            new HeaderPropagationValues(),
            new OptionsWrapper<ResourceEventOptions>(
                new ResourceEventOptions { ArnPrefix = "arn", TopicName = "topic-name" }
            ),
            NullLogger<ResourceEventPublisher>.Instance
        );

        await subject.Publish(
            new ResourceEventEntity
            {
                Id = "id",
                ResourceId = "resourceId",
                ResourceType = ResourceEventResourceTypes.CustomsDeclaration,
                Operation = "operation",
                Message = "message",
            },
            CancellationToken.None
        );

        await mockSimpleNotificationService
            .Received()
            .PublishAsync(
                Arg.Is<PublishRequest>(x =>
                    x.TopicArn == "arn:topic-name"
                    && x.MessageAttributes.ContainsKey("ResourceType")
                    && x.MessageAttributes["ResourceType"].StringValue == ResourceEventResourceTypes.CustomsDeclaration
                    && x.MessageAttributes.ContainsKey("ResourceId")
                    && x.MessageAttributes["ResourceId"].StringValue == "resourceId"
                    && x.Message == "message"
                ),
                CancellationToken.None
            );
    }

    [Fact]
    public async Task Publish_WhenNoResourceType_ShouldFail()
    {
        var mockSimpleNotificationService = Substitute.For<IAmazonSimpleNotificationService>();
        var subject = new ResourceEventPublisher(
            mockSimpleNotificationService,
            new OptionsWrapper<TraceHeader>(new TraceHeader { Name = "trace-id" }),
            new HeaderPropagationValues(),
            new OptionsWrapper<ResourceEventOptions>(
                new ResourceEventOptions { ArnPrefix = "arn", TopicName = "topic-name" }
            ),
            NullLogger<ResourceEventPublisher>.Instance
        );

        var act = async () =>
            await subject.Publish(
                new ResourceEventEntity
                {
                    Id = "id",
                    ResourceId = "resourceId",
                    ResourceType = "resourcetype",
                    Operation = "operation",
                    Message = "message",
                },
                CancellationToken.None
            );

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Publish_WhenEventIsLargerThanCompressionThreshold_ShouldCompressMessage()
    {
        var mockSimpleNotificationService = Substitute.For<IAmazonSimpleNotificationService>();
        var subject = new ResourceEventPublisher(
            mockSimpleNotificationService,
            new OptionsWrapper<TraceHeader>(new TraceHeader { Name = "trace-id" }),
            new HeaderPropagationValues(),
            new OptionsWrapper<ResourceEventOptions>(
                new ResourceEventOptions { ArnPrefix = "arn", TopicName = "topic-name" }
            ),
            NullLogger<ResourceEventPublisher>.Instance
        );

        const int largerThanCompressionThreshold = 256 * 1000 + 1;
        var sb = new StringBuilder(largerThanCompressionThreshold);
        const char pattern = 'A';
        for (var i = 0; i < largerThanCompressionThreshold; i++)
        {
            sb.Append(pattern);
        }
        var largeMessage = sb.ToString();
        largeMessage.Length.Should().Be(largerThanCompressionThreshold);

        var entity = new ResourceEventEntity
        {
            Id = "id",
            ResourceId = "resourceId",
            ResourceType = ResourceEventResourceTypes.CustomsDeclaration,
            Operation = "operation",
            Message = largeMessage,
        };

        await subject.Publish(entity, CancellationToken.None);

        await mockSimpleNotificationService
            .Received()
            .PublishAsync(
                Arg.Is<PublishRequest>(x =>
                    x.MessageAttributes["Content-Encoding"].StringValue == "gzip, base64"
                    && DecompressTo(x.Message) == largeMessage
                ),
                CancellationToken.None
            );
    }

    private static string DecompressTo(string compressedMessage)
    {
        compressedMessage.Length.Should().BeLessThan(256 * 1000);
        var compressedBytes = Convert.FromBase64String(compressedMessage);

        using var compressedStream = new MemoryStream(compressedBytes);
        using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var reader = new StreamReader(gzipStream, Encoding.UTF8);
        var result = reader.ReadToEnd();

        return result;
    }

    [Fact]
    public async Task Publish_WhenTraceIdPresent_ShouldBeIncluded()
    {
        var mockSimpleNotificationService = Substitute.For<IAmazonSimpleNotificationService>();
        var headerPropagationValues = new HeaderPropagationValues();
        var subject = new ResourceEventPublisher(
            mockSimpleNotificationService,
            new OptionsWrapper<TraceHeader>(new TraceHeader { Name = "trace-id" }),
            headerPropagationValues,
            new OptionsWrapper<ResourceEventOptions>(
                new ResourceEventOptions { ArnPrefix = "arn", TopicName = "topic-name" }
            ),
            NullLogger<ResourceEventPublisher>.Instance
        );

        headerPropagationValues.Headers = new Dictionary<string, StringValues> { { "trace-id", "trace-id-value" } };

        await subject.Publish(
            new ResourceEventEntity
            {
                Id = "id",
                ResourceId = "resourceId",
                ResourceType = ResourceEventResourceTypes.ProcessingError,
                Operation = "operation",
                Message = "message",
            },
            CancellationToken.None
        );

        await mockSimpleNotificationService
            .Received()
            .PublishAsync(
                Arg.Is<PublishRequest>(x =>
                    x.MessageAttributes.ContainsKey("trace-id")
                    && x.MessageAttributes["trace-id"].StringValue == "trace-id-value"
                ),
                CancellationToken.None
            );
    }

    [Fact]
    public async Task Publish_WhenSubResourceTypeSet_ShouldBeIncluded()
    {
        var mockSimpleNotificationService = Substitute.For<IAmazonSimpleNotificationService>();
        var subject = new ResourceEventPublisher(
            mockSimpleNotificationService,
            new OptionsWrapper<TraceHeader>(new TraceHeader { Name = "trace-id" }),
            new HeaderPropagationValues(),
            new OptionsWrapper<ResourceEventOptions>(
                new ResourceEventOptions { ArnPrefix = "arn", TopicName = "topic-name" }
            ),
            NullLogger<ResourceEventPublisher>.Instance
        );

        await subject.Publish(
            new ResourceEventEntity
            {
                Id = "id",
                ResourceId = "resourceId",
                ResourceType = ResourceEventResourceTypes.ImportPreNotification,
                SubResourceType = "subResourceType",
                Operation = "operation",
                Message = "message",
            },
            CancellationToken.None
        );

        await mockSimpleNotificationService
            .Received()
            .PublishAsync(
                Arg.Is<PublishRequest>(x =>
                    x.MessageAttributes.ContainsKey("SubResourceType")
                    && x.MessageAttributes["SubResourceType"].StringValue == "subResourceType"
                ),
                CancellationToken.None
            );
    }
}

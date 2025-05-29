using System.IO.Compression;
using System.Text;
using System.Text.Json;
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
            new ResourceEvent<FixtureEntity>
            {
                ResourceId = "resourceId",
                ResourceType = "resourceType",
                Operation = "operation",
                Timestamp = new DateTime(2025, 4, 16, 7, 0, 0, DateTimeKind.Utc),
            },
            CancellationToken.None
        );

        await mockSimpleNotificationService
            .Received()
            .PublishAsync(
                Arg.Is<PublishRequest>(x =>
                    x.TopicArn == "arn:topic-name"
                    && x.MessageAttributes.ContainsKey("ResourceType")
                    && x.MessageAttributes["ResourceType"].StringValue == "resourceType"
                    && x.Message
                        == "{\"resourceId\":\"resourceId\",\"resourceType\":\"resourceType\",\"subResourceType\":null,\"operation\":\"operation\",\"resource\":null,\"etag\":null,\"timestamp\":\"2025-04-16T07:00:00Z\",\"changeSet\":[]}"
                ),
                CancellationToken.None
            );
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

        var largerThanCompressionThreshold = 256 * 1000 + 1;
        var sb = new StringBuilder(largerThanCompressionThreshold);
        const char pattern = 'A';
        for (var i = 0; i < largerThanCompressionThreshold; i++)
        {
            sb.Append(pattern);
        }
        var largeMessage = sb.ToString();
        largeMessage.Length.Should().Be(largerThanCompressionThreshold);

        var resource = new FixtureEntity { Id = largeMessage };

        var @event = new ResourceEvent<FixtureEntity>
        {
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Resource = resource,
        };

        await subject.Publish(@event, CancellationToken.None);

        await mockSimpleNotificationService
            .Received()
            .PublishAsync(
                Arg.Is<PublishRequest>(x =>
                    x.MessageAttributes["Content-Encoding"].StringValue == "gzip, base64"
                    && DecodeAndDecompressTo<ResourceEvent<FixtureEntity>>(x.Message).Resource != resource
                ),
                CancellationToken.None
            );
    }

    private static T DecodeAndDecompressTo<T>(string compressedMessage)
    {
        compressedMessage.Length.Should().BeLessThan(256 * 1000);
        var compressedBytes = Convert.FromBase64String(compressedMessage);

        using var compressedStream = new MemoryStream(compressedBytes);
        using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var reader = new StreamReader(gzipStream, Encoding.UTF8);
        var result = reader.ReadToEnd();

        var deserialised = JsonSerializer.Deserialize<T>(result);
        deserialised.Should().NotBeNull();
        return deserialised;
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
            new ResourceEvent<FixtureEntity>
            {
                ResourceId = "resourceId",
                ResourceType = "resourceType",
                Operation = "operation",
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
            new ResourceEvent<FixtureEntity>
            {
                ResourceId = "resourceId",
                ResourceType = "resourceType",
                SubResourceType = "subResourceType",
                Operation = "operation",
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

    private class FixtureEntity : IDataEntity
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string ETag { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public void OnSave() { }
    }
}

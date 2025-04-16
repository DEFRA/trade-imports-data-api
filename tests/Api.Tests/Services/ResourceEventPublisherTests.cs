using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Defra.TradeImportsDataApi.Api.Configuration;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Microsoft.AspNetCore.HeaderPropagation;
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
            )
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
                    && x.MessageAttributes.ContainsKey("resourceType")
                    && x.MessageAttributes["resourceType"].StringValue == "resourceType"
                    && x.Message
                        == "{\"resourceId\":\"resourceId\",\"resourceType\":\"resourceType\",\"operation\":\"operation\",\"resource\":null,\"etag\":null,\"timestamp\":\"2025-04-16T07:00:00Z\",\"changeSet\":[]}"
                ),
                CancellationToken.None
            );
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
            )
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

using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Defra.TradeImportsDataApi.Api.Configuration;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Domain.Events;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.Extensions.Options;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ResourceEventPublisher(
    IAmazonSimpleNotificationService simpleNotificationService,
    IOptions<TraceHeader> traceHeaderOptions,
    HeaderPropagationValues headerPropagationValues,
    IOptions<ResourceEventOptions> resourceEventOptions
) : IResourceEventPublisher
{
    public async Task Publish<T>(ResourceEvent<T> @event, CancellationToken cancellationToken)
    {
        var messageAttributes = new Dictionary<string, MessageAttributeValue>
        {
            {
                "resourceType",
                new MessageAttributeValue { StringValue = @event.ResourceType, DataType = "String" }
            },
        };

        if (@event.ChildResourceType is not null)
        {
            messageAttributes.Add(
                "childResourceType",
                new MessageAttributeValue { StringValue = @event.ChildResourceType, DataType = "String" }
            );
        }

        AddTraceIdIfPresent(messageAttributes);

        var request = new PublishRequest
        {
            TopicArn = resourceEventOptions.Value.TopicArn,
            MessageAttributes = messageAttributes,
            Message = JsonSerializer.Serialize(@event),
        };

        await simpleNotificationService.PublishAsync(request, cancellationToken);
    }

    private void AddTraceIdIfPresent(Dictionary<string, MessageAttributeValue> messageAttributes)
    {
        if (
            headerPropagationValues.Headers != null
            && headerPropagationValues.Headers.TryGetValue(traceHeaderOptions.Value.Name, out var traceId)
        )
        {
            messageAttributes.Add(
                traceHeaderOptions.Value.Name,
                new MessageAttributeValue { StringValue = traceId, DataType = "String" }
            );
        }
    }
}

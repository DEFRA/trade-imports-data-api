using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Domain.Events;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.Extensions.Options;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ResourceEventPublisher(
    IAmazonSimpleNotificationService simpleNotificationService,
    IOptions<TraceHeader> traceHeader,
    HeaderPropagationValues headerPropagationValues
) : IResourceEventPublisher
{
    private string? _topicArn;

    public async Task Publish<T>(ResourceEvent<T> @event, CancellationToken cancellationToken)
    {
        var topicArn = await GetTopicArn();
        var messageAttributes = new Dictionary<string, MessageAttributeValue>
        {
            {
                "resourceType",
                new MessageAttributeValue { StringValue = @event.ResourceType, DataType = "String" }
            },
        };

        AddTraceIdIfPresent(messageAttributes);

        var request = new PublishRequest
        {
            TopicArn = topicArn,
            MessageAttributes = messageAttributes,
            Message = JsonSerializer.Serialize(@event),
        };

        await simpleNotificationService.PublishAsync(request, cancellationToken);
    }

    private void AddTraceIdIfPresent(Dictionary<string, MessageAttributeValue> messageAttributes)
    {
        if (
            headerPropagationValues.Headers != null
            && headerPropagationValues.Headers.TryGetValue(traceHeader.Value.Name, out var traceId)
        )
        {
            messageAttributes.Add(
                traceHeader.Value.Name,
                new MessageAttributeValue { StringValue = traceId, DataType = "String" }
            );
        }
    }

    private async Task<string> GetTopicArn()
    {
        if (_topicArn is not null)
            return _topicArn;

        // Topic name has not moved into config yet as it should remain
        // common throughout all environments
        var topic = await simpleNotificationService.FindTopicAsync("trade_imports_data_upserted");

        // Class registered as singleton so this serves as a cache and
        // therefore a one time lookup
        _topicArn = topic.TopicArn;

        return _topicArn;
    }
}

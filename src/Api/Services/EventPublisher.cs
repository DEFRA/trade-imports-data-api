using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.Extensions.Options;

namespace Defra.TradeImportsDataApi.Api.Services;

public class EventPublisher(
    IAmazonSimpleNotificationService simpleNotificationService,
    IOptions<TraceHeader> traceHeader,
    HeaderPropagationValues headerPropagationValues
) : IEventPublisher
{
    private string? _topicArn;

    public async Task Publish<T>(T @event, string resourceType, CancellationToken cancellationToken)
    {
        var topicArn = await GetTopicArn();
        var messageAttributes = new Dictionary<string, MessageAttributeValue>
        {
            {
                nameof(resourceType),
                new MessageAttributeValue { StringValue = resourceType, DataType = "String" }
            },
        };

        AddTraceIdIfPresent(messageAttributes);

        var request = new PublishRequest
        {
            MessageAttributes = messageAttributes,
            Message = JsonSerializer.Serialize(@event),
            TopicArn = topicArn,
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

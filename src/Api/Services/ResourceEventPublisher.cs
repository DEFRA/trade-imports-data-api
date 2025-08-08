using System.IO.Compression;
using System.Text;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Defra.TradeImportsDataApi.Api.Configuration;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.Extensions.Options;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ResourceEventPublisher(
    IAmazonSimpleNotificationService simpleNotificationService,
    IOptions<TraceHeader> traceHeaderOptions,
    HeaderPropagationValues headerPropagationValues,
    IOptions<ResourceEventOptions> resourceEventOptions,
    ILogger<ResourceEventPublisher> logger
) : IResourceEventPublisher
{
    private const string CompressedHeader = "Content-Encoding";
    private const int CompressionThreshold = 256 * 1000;
    private const string DataTypeString = "String";

    public async Task Publish(ResourceEventEntity entity, CancellationToken cancellationToken)
    {
        var (message, compressed) = SerializeEvent(entity);

        var messageAttributes = new Dictionary<string, MessageAttributeValue>
        {
            {
                nameof(entity.ResourceType),
                new MessageAttributeValue { StringValue = entity.ResourceType, DataType = DataTypeString }
            },
            {
                nameof(entity.ResourceId),
                new MessageAttributeValue { StringValue = entity.ResourceId, DataType = DataTypeString }
            },
        };

        if (entity.SubResourceType is not null)
        {
            messageAttributes.Add(
                nameof(entity.SubResourceType),
                new MessageAttributeValue { StringValue = entity.SubResourceType, DataType = DataTypeString }
            );
        }

        if (compressed)
        {
            messageAttributes.Add(
                CompressedHeader,
                new MessageAttributeValue { StringValue = "gzip, base64", DataType = DataTypeString }
            );
        }

        AddTraceIdIfPresent(messageAttributes);

        var request = new PublishRequest
        {
            TopicArn = resourceEventOptions.Value.TopicArn,
            MessageAttributes = messageAttributes,
            Message = message,
        };

        await simpleNotificationService.PublishAsync(request, cancellationToken);

        logger.LogInformation(
            "Published resource event {ResourceType} {Operation} {SubResourceType} (compressed {Compressed})",
            entity.ResourceType,
            entity.Operation,
            entity.SubResourceType,
            compressed
        );
    }

    private static (string message, bool compressed) SerializeEvent(ResourceEventEntity entity)
    {
        var message = entity.Message;
        if (message.Length <= CompressionThreshold)
            return (message, false);

        var buffer = Encoding.UTF8.GetBytes(message);
        var memoryStream = new MemoryStream();
        using var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal);
        gzipStream.Write(buffer, 0, buffer.Length);
        gzipStream.Flush();

        return (Convert.ToBase64String(memoryStream.ToArray()), true);
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
                new MessageAttributeValue { StringValue = traceId, DataType = DataTypeString }
            );
        }
    }
}

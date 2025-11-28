using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class SqsTestBase(ITestOutputHelper testOutputHelper) : IntegrationTestBase
{
    private const string QueueUrl =
        "http://sqs.eu-west-2.127.0.0.1:4566/000000000000/trade_imports_data_upserted_queue";

    private readonly AmazonSQSClient _sqsClient = new(
        new BasicAWSCredentials("test", "test"),
        new AmazonSQSConfig
        {
            AuthenticationRegion = "eu-west-2",
            ServiceURL = "http://localhost:4566",
            Timeout = TimeSpan.FromSeconds(5),
            MaxErrorRetry = 0,
        }
    );

    protected Task<ReceiveMessageResponse> ReceiveMessage()
    {
        return _sqsClient.ReceiveMessageAsync(
            new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 0,
            },
            CancellationToken.None
        );
    }

    protected Task<GetQueueAttributesResponse> GetQueueAttributes()
    {
        return _sqsClient.GetQueueAttributesAsync(
            new GetQueueAttributesRequest { AttributeNames = ["ApproximateNumberOfMessages"], QueueUrl = QueueUrl },
            CancellationToken.None
        );
    }

    protected async Task DrainAllMessages()
    {
        Assert.True(
            await AsyncWaiter.WaitForAsync(async () =>
            {
                var response = await ReceiveMessage();
                var messages = response.Messages ?? [];

                foreach (var message in messages)
                {
                    testOutputHelper?.WriteLine("Drain message: {0} {1}", message.MessageId, message.Body);

                    await _sqsClient.DeleteMessageAsync(
                        new DeleteMessageRequest { QueueUrl = QueueUrl, ReceiptHandle = message.ReceiptHandle },
                        CancellationToken.None
                    );
                }

                var approximateNumberOfMessages = (await GetQueueAttributes()).ApproximateNumberOfMessages;

                testOutputHelper?.WriteLine("ApproximateNumberOfMessages: {0}", approximateNumberOfMessages);

                return approximateNumberOfMessages == 0;
            })
        );
    }
}

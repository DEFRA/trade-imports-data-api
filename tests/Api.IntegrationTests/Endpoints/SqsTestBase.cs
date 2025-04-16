using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class SqsTestBase : IntegrationTestBase
{
    private const string QueueUrl =
        "http://sqs.eu-west-2.127.0.0.1:4566/000000000000/trade_imports_data_upserted_queue";

    private readonly AmazonSQSClient _sqsClient = new(
        new BasicAWSCredentials("test", "test"),
        new AmazonSQSConfig { AuthenticationRegion = "eu-west-2", ServiceURL = "http://localhost:4566" }
    );

    private Task<ReceiveMessageResponse> ReceiveMessage()
    {
        return _sqsClient.ReceiveMessageAsync(QueueUrl, CancellationToken.None);
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
                await ReceiveMessage();
                return (await GetQueueAttributes()).ApproximateNumberOfMessages == 0;
            })
        );
    }
}

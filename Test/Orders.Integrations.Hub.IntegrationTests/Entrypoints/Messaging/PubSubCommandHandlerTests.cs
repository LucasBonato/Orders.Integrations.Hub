using System.Text.Json;

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Aws;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Messaging;

[Collection(IntegrationTestCollection.Name)]
public sealed class PubSubCommandHandlerTests(
    TestInfrastructure infrastructure
) : IntegrationTestBase(infrastructure) {
    [Fact]
    public async Task Publish_ShouldPublishSerializedMessageToConfiguredSnsTopic()
    {
        // Arrange
        using AmazonSimpleNotificationServiceClient sns = LocalStackAwsClients.Sns(Infrastructure!.Environment);
        using AmazonSQSClient sqs = LocalStackAwsClients.Sqs(Infrastructure.Environment);
        CreateQueueResponse queue = await sqs.CreateQueueAsync(
            new CreateQueueRequest { QueueName = $"accept-order-{Guid.NewGuid():N}" },
            TestContext.Current.CancellationToken
        );
        SubscribeResponse subscription = await sns.SubscribeAsync(new SubscribeRequest {
            TopicArn = Infrastructure.Environment.SnsTopicArn,
            Protocol = "sqs",
            Endpoint = GetQueueArn(queue.QueueUrl)
        }, TestContext.Current.CancellationToken);
        SendNotificationCommand command = new SendNotificationCommandFaker()
            .WithoutTopicArn()
            .Generate();

        try
        {
            // Act
            using IServiceScope scope = Host.Services.CreateScope();
            IPublishEndpoint bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await bus.Publish(command, TestContext.Current.CancellationToken);

            ReceiveMessageResponse received = await sqs.ReceiveMessageAsync(new ReceiveMessageRequest {
                QueueUrl = queue.QueueUrl,
                WaitTimeSeconds = 20,
                MaxNumberOfMessages = 10
            }, TestContext.Current.CancellationToken);

            // Assert
            Message message = Assert.Single(received.Messages);
            using JsonDocument envelope = JsonDocument.Parse(message.Body);
            string serializedMessage = envelope.RootElement.GetProperty("Message").GetString()!;
            Assert.Contains(command.Message.OrderId, serializedMessage, StringComparison.Ordinal);
        }
        finally
        {
            await sns.UnsubscribeAsync(subscription.SubscriptionArn, TestContext.Current.CancellationToken);
            await sqs.DeleteQueueAsync(queue.QueueUrl, TestContext.Current.CancellationToken);
        }
    }

    private static string GetQueueArn(string queueUrl)
    {
        string[] parts = queueUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return $"arn:aws:sqs:us-east-1:{parts[^2]}:{parts[^1]}";
    }
}

using System.Text.Json;

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using MassTransit;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public sealed class PubSubCommandHandler(
    ILogger<PubSubCommandHandler> logger,
    IAmazonSimpleNotificationService simpleNotificationService
) : IConsumer<SendNotificationCommand> {
    public async Task Consume(ConsumeContext<SendNotificationCommand> context)
    {
        SendNotificationCommand command = context.Message;

        string shareConfirmOrderTopicArn = command.TopicArn ?? AppEnv.PUB_SUB.TOPICS.ACCEPT_ORDER.NotNullEnv();

        string message = JsonSerializer.Serialize(command.Message);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Publishing payload {payload} to topic {topicArn}", message, shareConfirmOrderTopicArn);

        PublishResponse? messageId = await simpleNotificationService.PublishAsync(
            shareConfirmOrderTopicArn, 
            message, 
            context.CancellationToken
        );

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Message [{messageId}] sent to topic [{topicArn}] for order [{orderId}]",
                messageId.MessageId,
                shareConfirmOrderTopicArn,
                command.Message.OrderId
            );
    }
}
using System.Text.Json;

using Amazon.SimpleNotificationService;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class PubSubEventHandler(
    ILogger<PubSubEventHandler> logger,
    IAmazonSimpleNotificationService simpleNotificationService
) : IEventHandler<SendNotificationCommand> {
    public async Task HandleAsync(SendNotificationCommand commandModel, CancellationToken ct)
    {
        var shareConfirmOrderTopicArn = commandModel.TopicArn ?? AppEnv.PUB_SUB.TOPICS.ACCEPT_ORDER.NotNullEnv();

        string message = JsonSerializer.Serialize(commandModel.Message);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Publishing payload {payload} to topic {topicArn}", message, shareConfirmOrderTopicArn);

        var messageId = await simpleNotificationService.PublishAsync(shareConfirmOrderTopicArn, message, ct);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Message [{messageId}] sent to topic [{topicArn}] for order [{orderId}]",
                messageId.MessageId,
                shareConfirmOrderTopicArn,
                commandModel.Message.OrderId
            );
    }
}
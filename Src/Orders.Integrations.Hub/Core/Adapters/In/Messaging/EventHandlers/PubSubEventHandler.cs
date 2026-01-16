using System.Text.Json;

using Amazon.SimpleNotificationService;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class PubSubEventHandler(
    ILogger<PubSubEventHandler> logger,
    IAmazonSimpleNotificationService simpleNotificationService
) : IEventHandler<SendNotificationEvent> {
    public async Task HandleAsync(SendNotificationEvent eventModel, CancellationToken ct)
    {
        var shareConfirmOrderTopicArn = eventModel.TopicArn ?? AppEnv.PUB_SUB.TOPICS.ACCEPT_ORDER.NotNullEnv();

        string message = JsonSerializer.Serialize(eventModel.Message);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Publishing payload {payload} to topic {topicArn}", message, shareConfirmOrderTopicArn);

        var messageId = await simpleNotificationService.PublishAsync(shareConfirmOrderTopicArn, message, ct);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Message [{messageId}] sent to topic [{topicArn}] for order [{orderId}]",
                messageId.MessageId,
                shareConfirmOrderTopicArn,
                eventModel.Message.OrderId
            );
    }
}
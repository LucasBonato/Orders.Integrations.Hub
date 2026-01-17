using System.Text.Json;

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class PubSubCommandHandler(
    ILogger<PubSubCommandHandler> logger,
    IAmazonSimpleNotificationService simpleNotificationService
) : IEventHandler<FastEndpointsCommandEnvelope<SendNotificationCommand>> {
    public async Task HandleAsync(FastEndpointsCommandEnvelope<SendNotificationCommand> envelope, CancellationToken ct)
    {
        SendNotificationCommand command = envelope.Command;

        string shareConfirmOrderTopicArn = command.TopicArn ?? AppEnv.PUB_SUB.TOPICS.ACCEPT_ORDER.NotNullEnv();

        string message = JsonSerializer.Serialize(command.Message);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Publishing payload {payload} to topic {topicArn}", message, shareConfirmOrderTopicArn);

        PublishResponse? messageId = await simpleNotificationService.PublishAsync(shareConfirmOrderTopicArn, message, ct);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Message [{messageId}] sent to topic [{topicArn}] for order [{orderId}]",
                messageId.MessageId,
                shareConfirmOrderTopicArn,
                command.Message.OrderId
            );
    }
}
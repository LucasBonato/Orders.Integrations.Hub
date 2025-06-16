using System.Text.Json;

using Amazon.SimpleNotificationService;

using BizPik.Orders.Hub.Core.Application.Extensions;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Application.EventHandlers;

public class PubSubEventHandler(
    ILogger<PubSubEventHandler> logger,
    IAmazonSimpleNotificationService simpleNotificationService
) : IEventHandler<SendNotificationEvent> {
    public async Task HandleAsync(SendNotificationEvent eventModel, CancellationToken ct)
    {
        var shareConfirmOrderTopicArn = eventModel.TopicArn ?? AppEnv.PUB_SUB.TOPICS.ACCEPT_ORDER.NotNullEnv();

        string message = JsonSerializer.Serialize(eventModel.Message);

        logger.LogInformation("Publishing payload {payload} to topic {topicArn}", message, shareConfirmOrderTopicArn);

        var messageId = await simpleNotificationService.PublishAsync(shareConfirmOrderTopicArn, message, ct);

        logger.LogInformation("Message [{messageId}] sent to topic [{topicArn}] for order [{orderId}]",
            messageId.MessageId,
            shareConfirmOrderTopicArn,
            eventModel.Message.OrderId
        );
    }
}
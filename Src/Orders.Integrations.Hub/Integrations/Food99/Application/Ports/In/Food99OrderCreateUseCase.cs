using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;

public class Food99OrderCreateUseCase(
    IIntegrationContext integrationContext
) : IOrderCreateUseCase<Food99WebhookRequest> {
    public async Task<Food99WebhookRequest> ExecuteAsync(Food99WebhookRequest requestOrder) {
        string tenantId = integrationContext.Integration!.TenantId ?? string.Empty;

        await new CreateOrderEvent(
            Order: requestOrder.ToOrder(tenantId),
            SalesChannel: Food99IntegrationKey.FOOD99
        ).PublishAsync();

        if (integrationContext.Integration.AutoAccept) {
            await new SendNotificationEvent(
                Message: requestOrder.FromFood99(OrderEventType.CONFIRMED),
                TopicArn: null
            ).PublishAsync();
        }

        return requestOrder;
    }
}
using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;

public class Food99OrderCreateUseCase(
    IIntegrationContext integrationContext
) : IOrderCreateUseCase<Food99WebhookRequest> {
    public async Task<Food99WebhookRequest> ExecuteAsync(Food99WebhookRequest requestOrder) {
        int tenantId = integrationContext.Integration!.TenantId ?? 0;

        await new CreateOrderEvent(
            Order: requestOrder.ToOrder(tenantId),
            SalesChannel: OrderSalesChannel.FOOD99
        ).PublishAsync();

        if (integrationContext.Integration.Resolve99Food().AutoAccept) {
            await new SendNotificationEvent(
                Message: requestOrder.FromFood99(OrderEventType.CONFIRMED),
                TopicArn: null
            ).PublishAsync();
        }

        return requestOrder;
    }
}
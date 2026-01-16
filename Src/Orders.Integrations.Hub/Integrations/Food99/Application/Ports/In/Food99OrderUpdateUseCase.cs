using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Integrations.Food99.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;

public class Food99OrderUpdateUseCase : IOrderUpdateUseCase<Food99WebhookRequest>
{
    public async Task<Food99WebhookRequest> ExecuteAsync(Food99WebhookRequest integrationOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdate: integrationOrder.FromFood99(null),
            SalesChannel: Food99IntegrationKey.FOOD99
        ).PublishAsync();

        return integrationOrder;
    }
}
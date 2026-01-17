using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Food99.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;

public class Food99ApplyOrderDisputeUseCase(
    ICommandDispatcher dispatcher
) : IOrderDisputeUseCase<Food99WebhookRequest> {
    public async Task<Food99WebhookRequest> ExecuteAsync(Food99WebhookRequest order) {
        await dispatcher.DispatchAsync(
            new ProcessOrderDisputeCommand(
                ExternalOrderId: order.Data.OrderId.ToString(),
                Integration: Food99IntegrationKey.FOOD99,
                OrderDispute: order.ToOrderDispute(),
                Type: OrderEventType.DISPUTE_STARTED
            )
        );

        return order;
    }
}
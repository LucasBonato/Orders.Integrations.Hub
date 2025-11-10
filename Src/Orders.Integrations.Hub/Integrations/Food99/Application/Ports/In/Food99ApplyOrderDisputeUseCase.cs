using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Food99.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;

public class Food99ApplyOrderDisputeUseCase : IOrderDisputeUseCase<Food99WebhookRequest> {
    public async Task<Food99WebhookRequest> ExecuteAsync(Food99WebhookRequest order) {
        await new ProcessOrderDisputeEvent(
            ExternalOrderId: order.Data.OrderId.ToString(),
            Integration: OrderIntegration.FOOD99,
            OrderDispute: order.ToOrderDispute(),
            Type: OrderEventType.DISPUTE_STARTED
        ).PublishAsync();

        return order;

    }
}
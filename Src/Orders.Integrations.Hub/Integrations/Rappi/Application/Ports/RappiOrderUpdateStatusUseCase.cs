using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports;

public class RappiOrderUpdateStatusUseCase : IOrderUpdateStatusUseCase<RappiWebhookEventOrderRequest> {
    public async Task<RappiWebhookEventOrderRequest> ExecuteAsync(RappiWebhookEventOrderRequest requestOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdateStatus: requestOrder.FromRappi(),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        return requestOrder;
    }
}
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;

public class RappiOrderUpdateUseCase : IOrderUpdateUseCase<RappiWebhookEventOrderRequest> {
    public async Task<RappiWebhookEventOrderRequest> ExecuteAsync(RappiWebhookEventOrderRequest requestOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdate: requestOrder.FromRappi(),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        return requestOrder;
    }
}
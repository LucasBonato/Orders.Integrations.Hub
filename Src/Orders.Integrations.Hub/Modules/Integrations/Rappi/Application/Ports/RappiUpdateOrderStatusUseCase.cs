using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Integrations.Rappi.Application.Ports;

public class RappiUpdateOrderStatusUseCase(
    ILogger<RappiUpdateOrderStatusUseCase> logger
) : IUpdateOrderStatusUseCase<RappiWebhookEventOrderRequest> {
    public async Task<RappiWebhookEventOrderRequest> ExecuteAsync(RappiWebhookEventOrderRequest requestOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdateStatus: requestOrder.FromRappi(),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        return requestOrder;
    }
}
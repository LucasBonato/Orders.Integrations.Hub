using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace BizPik.Orders.Hub.Integrations.Rappi.Application.Ports.In;

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
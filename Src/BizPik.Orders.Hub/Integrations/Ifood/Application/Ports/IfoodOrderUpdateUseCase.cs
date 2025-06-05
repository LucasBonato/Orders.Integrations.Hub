using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Ports;

public class IfoodOrderUpdateUseCase : IOrderUpdateUseCase<IfoodWebhookRequest>
{
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest ifoodOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdate: ifoodOrder.FromIfood(null),
            SalesChannel: OrderSalesChannel.IFOOD
        ).PublishAsync();

        return await Task.FromResult(ifoodOrder);
    }
}
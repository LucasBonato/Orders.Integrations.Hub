using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Extensions;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderUpdateStatusUseCase : IOrderUpdateStatusUseCase<IfoodWebhookRequest>
{
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest ifoodOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdateStatus: ifoodOrder.FromIfood(null),
            SalesChannel: OrderSalesChannel.IFOOD
        ).PublishAsync();

        return await Task.FromResult(ifoodOrder);
    }
}
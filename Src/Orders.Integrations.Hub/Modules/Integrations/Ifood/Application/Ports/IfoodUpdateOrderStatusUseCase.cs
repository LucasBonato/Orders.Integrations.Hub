using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Extensions;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodUpdateOrderStatusUseCase : IUpdateOrderStatusUseCase<IfoodWebhookRequest>
{
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest ifoodOrder)
    {
        await new UpdateOrderStatusEvent() {
            OrderUpdateStatus = ifoodOrder.FromIfood(null),
            SalesChannel = OrderSalesChannel.IFOOD
        }.PublishAsync();

        return await Task.FromResult(ifoodOrder);
    }
}
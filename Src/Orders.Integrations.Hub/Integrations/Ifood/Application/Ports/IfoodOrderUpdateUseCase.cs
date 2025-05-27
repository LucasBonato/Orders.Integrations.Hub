using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports;

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
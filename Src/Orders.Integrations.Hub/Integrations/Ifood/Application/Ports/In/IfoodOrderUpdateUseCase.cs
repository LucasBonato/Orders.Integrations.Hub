using Orders.Integrations.Hub.Integrations.Ifood.Application.Extensions;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports.In;

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
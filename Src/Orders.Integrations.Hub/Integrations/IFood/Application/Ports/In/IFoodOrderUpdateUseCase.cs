using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;

public class IFoodOrderUpdateUseCase : IOrderUpdateUseCase<IFoodWebhookRequest>
{
    public async Task<IFoodWebhookRequest> ExecuteAsync(IFoodWebhookRequest foodOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdate: foodOrder.FromIFood(null),
            SalesChannel: OrderSalesChannel.IFOOD
        ).PublishAsync();

        return await Task.FromResult(foodOrder);
    }
}
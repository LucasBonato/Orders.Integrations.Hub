using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;

public class IFoodOrderUpdateUseCase(
    ICommandDispatcher dispatcher
) : IOrderUpdateUseCase<IFoodWebhookRequest> {
    public async Task<IFoodWebhookRequest> ExecuteAsync(IFoodWebhookRequest foodOrder)
    {
        await dispatcher.DispatchAsync(
            new UpdateOrderStatusCommand(
                OrderUpdate: foodOrder.FromIFood(null),
                SalesChannel: IFoodIntegrationKey.IFOOD
            )
        );

        return await Task.FromResult(foodOrder);
    }
}
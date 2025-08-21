using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;

public class IFoodOrderCreateUseCase(
    IIFoodClient iFoodClient
) : IOrderCreateUseCase<IFoodWebhookRequest> {
    public async Task<IFoodWebhookRequest> ExecuteAsync(IFoodWebhookRequest requestOrder)
    {
        IFoodOrder foodOrder = await iFoodClient.GetOrderDetails(requestOrder.OrderId);

        const int companyId = 0;

        await new CreateOrderEvent(
            Order: foodOrder.ToOrder(companyId),
            SalesChannel: OrderSalesChannel.IFOOD
        ).PublishAsync();

        const bool isAutoAccept = false;

        if (isAutoAccept)
        {
            await new SendNotificationEvent(
                Message: requestOrder.FromIFood(OrderEventType.CONFIRMED),
                TopicArn: null
            ).PublishAsync();
        }

        return requestOrder;
    }
}
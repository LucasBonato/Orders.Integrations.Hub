using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports.In;

public class IfoodOrderCreateUseCase(
    IIFoodClient iFoodClient
) : IOrderCreateUseCase<IfoodWebhookRequest> {
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest requestOrder)
    {
        IfoodOrder ifoodOrder = await iFoodClient.GetOrderDetails(requestOrder.OrderId);

        const int companyId = 0;

        await new CreateOrderEvent(
            Order: ifoodOrder.ToOrder(companyId),
            SalesChannel: OrderSalesChannel.IFOOD
        ).PublishAsync();

        const bool isAutoAccept = false;

        if (isAutoAccept)
        {
            await new SendNotificationEvent(
                Message: requestOrder.FromIfood(OrderEventType.CONFIRMED),
                TopicArn: null
            ).PublishAsync();
        }

        return requestOrder;
    }
}
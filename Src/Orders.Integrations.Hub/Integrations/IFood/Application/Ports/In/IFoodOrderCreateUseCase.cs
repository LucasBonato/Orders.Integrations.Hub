using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;

public class IFoodOrderCreateUseCase(
    IIntegrationContext integrationContext,
    IIFoodClient iFoodClient
) : IOrderCreateUseCase<IFoodWebhookRequest> {
    public async Task<IFoodWebhookRequest> ExecuteAsync(IFoodWebhookRequest requestOrder)
    {
        IFoodOrder foodOrder = await iFoodClient.GetOrderDetails(requestOrder.OrderId);

        string tenantId = integrationContext.Integration!.TenantId?? string.Empty;

        await new CreateOrderEvent(
            Order: foodOrder.ToOrder(tenantId),
            SalesChannel: OrderSalesChannel.IFOOD
        ).PublishAsync();

        if (integrationContext.Integration.AutoAccept)
        {
            await new SendNotificationEvent(
                Message: requestOrder.FromIFood(OrderEventType.CONFIRMED),
                TopicArn: null
            ).PublishAsync();
        }

        return requestOrder;
    }
}
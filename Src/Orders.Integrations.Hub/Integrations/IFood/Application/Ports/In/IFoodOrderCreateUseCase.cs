using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;

public class IFoodOrderCreateUseCase(
    IIntegrationContext integrationContext,
    ICommandDispatcher dispatcher,
    IIFoodClient iFoodClient
) : IOrderCreateUseCase<IFoodWebhookRequest> {
    public async Task<IFoodWebhookRequest> ExecuteAsync(IFoodWebhookRequest requestOrder)
    {
        IFoodOrder foodOrder = await iFoodClient.GetOrderDetails(requestOrder.OrderId);

        string tenantId = integrationContext.Integration!.TenantId?? string.Empty;

        await dispatcher.DispatchAsync(
            new CreateOrderCommand(
                Order: foodOrder.ToOrder(tenantId)
            )
        );

        if (integrationContext.Integration.AutoAccept)
        {
            await dispatcher.DispatchAsync(
                new SendNotificationCommand(
                    Message: requestOrder.FromIFood(OrderEventType.CONFIRMED),
                    TopicArn: null
                )
            );
        }

        return requestOrder;
    }
}
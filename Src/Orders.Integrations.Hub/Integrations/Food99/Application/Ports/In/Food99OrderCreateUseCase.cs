using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;

public class Food99OrderCreateUseCase(
    IIntegrationContext integrationContext,
    ICommandDispatcher dispatcher
) : IOrderCreateUseCase<Food99WebhookRequest> {
    public async Task<Food99WebhookRequest> ExecuteAsync(Food99WebhookRequest requestOrder) {
        string tenantId = integrationContext.Integration!.TenantId ?? string.Empty;

        await dispatcher.DispatchAsync(
            new CreateOrderCommand(
                Order: requestOrder.ToOrder(tenantId)
            )
        );

        if (integrationContext.Integration.AutoAccept) {
            await dispatcher.DispatchAsync(
                new SendNotificationCommand(
                    Message: requestOrder.FromFood99(OrderEventType.CONFIRMED),
                    TopicArn: null
                )
            );
        }

        return requestOrder;
    }
}
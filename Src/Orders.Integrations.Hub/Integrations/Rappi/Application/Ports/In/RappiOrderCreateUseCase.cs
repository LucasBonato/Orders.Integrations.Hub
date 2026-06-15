using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;

public class RappiOrderCreateUseCase(
    IIntegrationContext integrationContext,
    ICommandDispatcher dispatcher
) : IOrderCreateUseCase<RappiOrder> {
    public async Task<RappiOrder> ExecuteAsync(RappiOrder request)
    {
        string tenantId = integrationContext.Integration!.TenantId?? string.Empty;

        await dispatcher.DispatchAsync(
            new CreateOrderCommand(
                Order: request.ToOrder(tenantId)
            )
        );

        if (integrationContext.Integration.AutoAccept)
        {
            await dispatcher.DispatchAsync(
                new SendNotificationCommand(
                    Message: request.FromRappi(OrderEventType.CONFIRMED),
                    TopicArn: null
                )
            );
        }

        return request;
    }
}
using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;

public class RappiOrderUpdateUseCase(
    ICommandDispatcher dispatcher
) : IOrderUpdateUseCase<RappiWebhookEventOrderRequest> {
    public async Task<RappiWebhookEventOrderRequest> ExecuteAsync(RappiWebhookEventOrderRequest requestOrder)
    {
        await dispatcher.DispatchAsync(
            new UpdateOrderStatusCommand(
                OrderUpdate: requestOrder.FromRappi(),
                SalesChannel: RappiIntegrationKey.RAPPI
            )
        );

        return requestOrder;
    }
}
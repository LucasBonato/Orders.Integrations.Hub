using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;

public class RappiOrderUpdateUseCase : IOrderUpdateUseCase<RappiWebhookEventOrderRequest> {
    public async Task<RappiWebhookEventOrderRequest> ExecuteAsync(RappiWebhookEventOrderRequest requestOrder)
    {
        await new UpdateOrderStatusEvent(
            OrderUpdate: requestOrder.FromRappi(),
            SalesChannel: RappiIntegrationKey.RAPPI
        ).PublishAsync();

        return requestOrder;
    }
}
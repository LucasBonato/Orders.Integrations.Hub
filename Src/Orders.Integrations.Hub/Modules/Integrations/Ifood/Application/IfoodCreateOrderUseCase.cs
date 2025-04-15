using Orders.Integrations.Hub.Modules.Common.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Modules.Integrations.Common.Application;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application;

public class IfoodCreateOrderUseCase : ICreateOrderUseCase<IfoodWebhookRequest>
{
    public Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest integrationOrder)
    {
        // new CreateOrderEvent() {
        //     Order = ,
        //     SalesChannel = ,
        // }.PublishAsync();

        return Task.Run(() => integrationOrder);
    }
}
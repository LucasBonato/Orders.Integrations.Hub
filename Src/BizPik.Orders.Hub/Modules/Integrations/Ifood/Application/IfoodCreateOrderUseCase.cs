using BizPik.Orders.Hub.Modules.Common.Orders.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Modules.Integrations.Common.Application;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application;

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
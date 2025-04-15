using BizPik.Orders.Hub.Modules.Integrations.Common.Application;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application;

public class IfoodUpdateOrderStatusUseCase : IUpdateOrderStatusUseCase<IfoodWebhookRequest>
{
    public Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest integrationOrder)
    {
        throw new NotImplementedException();
    }
}
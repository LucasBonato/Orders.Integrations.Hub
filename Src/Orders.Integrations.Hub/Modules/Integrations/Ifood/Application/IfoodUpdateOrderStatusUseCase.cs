using Orders.Integrations.Hub.Modules.Integrations.Common.Application;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application;

public class IfoodUpdateOrderStatusUseCase : IUpdateOrderStatusUseCase<IfoodWebhookRequest>
{
    public Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest integrationOrder)
    {
        throw new NotImplementedException();
    }
}
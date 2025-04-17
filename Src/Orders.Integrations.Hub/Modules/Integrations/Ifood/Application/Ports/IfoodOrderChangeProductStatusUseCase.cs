using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderChangeProductStatusUseCase(
    IIFoodClient ifoodClient
) : IOrderChangeProductStatusUseCase {
    public OrderIntegration Integration => OrderIntegration.IFOOD;

    public Task Enable(SNSProductEvent productEvent)
    {
        throw new NotImplementedException();
    }

    public Task Disable(SNSProductEvent productEvent)
    {
        throw new NotImplementedException();
    }
}
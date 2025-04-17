using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderChangeProductStatusUseCase(
    IIFoodClient ifoodClient
) : IOrderChangeProductStatusUseCase {
    public OrderIntegration Integration => OrderIntegration.IFOOD;

    public Task Enable(BizPikSNSProductEvent productEvent)
    {
        throw new NotImplementedException();
    }

    public Task Disable(BizPikSNSProductEvent productEvent)
    {
        throw new NotImplementedException();
    }
}
using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    OrderIntegration Integration { get; }
    Task Enable(BizPikSNSProductEvent productEvent);
    Task Disable(BizPikSNSProductEvent productEvent);
}
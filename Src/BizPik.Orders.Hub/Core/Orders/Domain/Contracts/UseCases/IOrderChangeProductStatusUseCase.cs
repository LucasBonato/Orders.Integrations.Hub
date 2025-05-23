using BizPik.Orders.Hub.Core.BizPik.Domain.ValueObjects;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(BizPikSNSProductEvent productEvent);
    Task Disable(BizPikSNSProductEvent productEvent);
}
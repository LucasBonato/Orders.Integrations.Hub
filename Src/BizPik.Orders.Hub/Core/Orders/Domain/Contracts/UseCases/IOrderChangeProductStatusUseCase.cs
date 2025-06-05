using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.BizPik;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(BizPikSNSProductEvent productEvent);
    Task Disable(BizPikSNSProductEvent productEvent);
}
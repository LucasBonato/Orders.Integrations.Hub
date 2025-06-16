using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;

namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(BizPikSNSProductEvent productEvent);
    Task Disable(BizPikSNSProductEvent productEvent);
}
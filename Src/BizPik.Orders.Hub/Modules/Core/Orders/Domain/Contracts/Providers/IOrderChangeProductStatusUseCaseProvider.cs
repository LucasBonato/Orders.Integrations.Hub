using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.Providers;

public interface IOrderChangeProductStatusUseCaseProvider
{
    IOrderChangeProductStatusUseCase Get(OrderIntegration integration);
}
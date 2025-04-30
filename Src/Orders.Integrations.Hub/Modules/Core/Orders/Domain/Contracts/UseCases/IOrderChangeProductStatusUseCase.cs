using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    OrderIntegration Integration { get; }
    Task Enable(SNSProductEvent productEvent);
    Task Disable(SNSProductEvent productEvent);
}
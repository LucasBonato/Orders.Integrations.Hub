using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(SNSProductEvent productEvent);
    Task Disable(SNSProductEvent productEvent);
}
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(SNSProductEvent productEvent);
    Task Disable(SNSProductEvent productEvent);
}
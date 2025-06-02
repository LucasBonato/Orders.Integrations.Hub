using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderDisputeRespondUseCase
{
    Task ExecuteAsync(RespondDisputeIntegrationRequest respondRequest);
}
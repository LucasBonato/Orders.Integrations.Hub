using Orders.Integrations.Hub.Core.Application.DTOs.Request;

namespace Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;

public interface IOrderDisputeRespondUseCase
{
    Task ExecuteAsync(RespondDisputeIntegrationRequest respondRequest);
}
using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderDisputeRespondUseCase
{
    Task ExecuteAsync(RespondDisputeIntegrationRequest respondRequest);
}
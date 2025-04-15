namespace BizPik.Orders.Hub.Modules.Integrations.Common.Domain.Contracts;

public interface IIntegrationAuthClient<in TRequest, TResponse>
{
    Task<TResponse> RetrieveToken(TRequest request);
}
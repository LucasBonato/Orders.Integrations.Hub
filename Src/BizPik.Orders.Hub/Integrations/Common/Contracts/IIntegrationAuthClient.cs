namespace BizPik.Orders.Hub.Integrations.Common.Contracts;

public interface IIntegrationAuthClient<in TRequest, TResponse>
{
    Task<TResponse> RetrieveToken(TRequest request);
}
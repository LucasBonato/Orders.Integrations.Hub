namespace Orders.Integrations.Hub.Integrations.Common.Contracts;

public interface IIntegrationAuthClient<in TRequest, TResponse>
{
    Task<TResponse> RetrieveToken(TRequest request);
    Task<TResponse> RefreshToken(TRequest request);
}
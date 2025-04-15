using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;

namespace Orders.Integrations.Hub.Modules.Core..Domain.Contracts;

public interface IInternalClient
{
    Task<ResponseWrapper<IntegrationResponse>> GetIntegrationByExternalId(string externalId, string apiKey);
    Task<ResponseWrapper<OnlineStoresResponse>> OnlineStores(string[] merchantIds);
}
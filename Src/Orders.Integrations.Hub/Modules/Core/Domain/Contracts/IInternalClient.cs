using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;

namespace Orders.Integrations.Hub.Modules.Core..Domain.Contracts;

public interface IInternalClient
{
    Task<ResponseWrapper<IntegrationResponse>> GetIntegrationByExternalId(string externalId);
    Task<ResponseWrapper<CompanyDetailsResponse>> GetCompanyByExternalId(string externalId);
    Task<ResponseWrapper<List<IntegrationResponse>>> GetIntegrationByCompanyId(string companyId);
    Task<ResponseWrapper<OnlineStoresResponse>> OnlineStores(string[] merchantIds);
}
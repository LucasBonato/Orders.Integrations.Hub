using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;

public interface IBizPikMonolithClient
{
    Task<BizPikResponseWrapper<BizPikIntegrationResponse>> GetIntegrationByExternalId(string externalId);
    Task<BizPikResponseWrapper<BizPikCompanyDetailsResponse>> GetCompanyByExternalId(string externalId);
    Task<BizPikResponseWrapper<List<BizPikIntegrationResponse>>> GetIntegrationByCompanyId(string companyId);
    Task<BizPikResponseWrapper<BizPikOnlineStoresResponse>> OnlineStores(string[] merchantIds);
}
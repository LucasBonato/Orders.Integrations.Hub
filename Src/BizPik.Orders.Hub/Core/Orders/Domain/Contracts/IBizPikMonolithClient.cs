using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.BizPik;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts;

public interface IBizPikMonolithClient
{
    Task<BizPikResponseWrapper<BizPikIntegrationResponse>> GetIntegrationByExternalId(string externalId);
    Task<BizPikResponseWrapper<BizPikCompanyDetailsResponse>> GetCompanyByExternalId(string externalId);
    Task<BizPikResponseWrapper<List<BizPikIntegrationResponse>>> GetIntegrationByCompanyId(string companyId);
    Task<BizPikResponseWrapper<BizPikOnlineStoresResponse>> OnlineStores(string[] merchantIds);
}
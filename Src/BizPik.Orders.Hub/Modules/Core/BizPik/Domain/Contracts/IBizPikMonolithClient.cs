using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;

public interface IBizPikMonolithClient
{
    Task<BizPikResponseWrapper<BizPikIntegrationResponse>> GetIntegrationByExternalId(string externalId, string apiKey);
    Task<BizPikResponseWrapper<BizPikOnlineStoresResponse>> OnlineStores(string[] merchantIds);
}
using System.Text.Json;

using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Application;

public class BizPikMonolithClient(
    ILogger<BizPikMonolithClient> logger,
    HttpClient httpClient
) : IBizPikMonolithClient {
    public async Task<BizPikResponseWrapper<BizPikIntegrationResponse>> GetIntegrationByExternalId(string externalId, string apiKey)
    {
        string uri = $"companies/lambda/integrations-details/external-id/{externalId}";
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - BizPikMonolithClient - Error trying to get the integration by externalId: {externalId}; apiKey: {apiKey}", externalId, apiKey);
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<BizPikResponseWrapper<BizPikIntegrationResponse>>())!;
    }

    public async Task<BizPikResponseWrapper<BizPikOnlineStoresResponse>> OnlineStores(string[] merchantIds)
    {
        string uri = "stores/open";
        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(JsonSerializer.Serialize(merchantIds)));
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - BizPikMonolithClient - Error trying to send merchantIds online: {merchantIds}", merchantIds);
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<BizPikResponseWrapper<BizPikOnlineStoresResponse>>())!;
    }
}
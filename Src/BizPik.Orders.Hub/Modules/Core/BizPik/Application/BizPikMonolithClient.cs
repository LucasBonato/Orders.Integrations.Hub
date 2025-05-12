using System.Text.Json;

using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Application;

public class BizPikMonolithClient(
    ILogger<BizPikMonolithClient> logger,
    HttpClient httpClient
) : IBizPikMonolithClient {
    string apiKey = AppEnv.BIZPIK.MONOLITH.API_KEYS.COMPANIES_INTEGRATIONS.NotNull();

    public async Task<BizPikResponseWrapper<BizPikIntegrationResponse>> GetIntegrationByExternalId(string externalId)
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

    public async Task<BizPikResponseWrapper<BizPikCompanyDetailsResponse>> GetCompanyByExternalId(string externalId)
    {
        string uri = $"companies/lambda/integrations/external-id/{externalId}";
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - BizPikMonolithClient - Error trying to get the company by externalId: {externalId}; apiKey: {apiKey}", externalId, apiKey);
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<BizPikResponseWrapper<BizPikCompanyDetailsResponse>>())!;
    }

    public async Task<BizPikResponseWrapper<List<BizPikIntegrationResponse>>> GetIntegrationByCompanyId(string companyId)
    {
        string uri = $"integrations/company/{companyId}";
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - BizPikMonolithClient - Error trying to get the integration by externalId: {companyId}; apiKey: {apiKey}", companyId, apiKey);
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<BizPikResponseWrapper<List<BizPikIntegrationResponse>>>())!;
    }

    public async Task<BizPikResponseWrapper<BizPikOnlineStoresResponse>> OnlineStores(string[] merchantIds)
    {
        string uri = "stores/open";
        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(JsonSerializer.Serialize(merchantIds)));
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - BizPikMonolithClient - Error trying to send merchantIds online: {merchantIds}", merchantIds.ToList());
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<BizPikResponseWrapper<BizPikOnlineStoresResponse>>())!;
    }
}
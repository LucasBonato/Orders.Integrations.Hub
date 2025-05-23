using System.Text.Json;

using Orders.Integrations.Hub.Core..Domain.Contracts;
using Orders.Integrations.Hub.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Core.Orders.Application.Extensions;

namespace Orders.Integrations.Hub.Core..Application;

public class InternalClient(
    ILogger<InternalClient> logger,
    HttpClient httpClient
) : IInternalClient {
    string apiKey = AppEnv..MONOLITH.API_KEYS.COMPANIES_INTEGRATIONS.NotNullEnv();

    public async Task<ResponseWrapper<IntegrationResponse>> GetIntegrationByExternalId(string externalId)
    {
        string uri = $"companies/lambda/integrations-details/external-id/{externalId}";
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - InternalClient - Error trying to get the integration by externalId: {externalId}; apiKey: {apiKey}", externalId, apiKey);
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<ResponseWrapper<IntegrationResponse>>())!;
    }

    public async Task<ResponseWrapper<CompanyDetailsResponse>> GetCompanyByExternalId(string externalId)
    {
        string uri = $"companies/lambda/integrations/external-id/{externalId}";
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - InternalClient - Error trying to get the company by externalId: {externalId}; apiKey: {apiKey}", externalId, apiKey);
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<ResponseWrapper<CompanyDetailsResponse>>())!;
    }

    public async Task<ResponseWrapper<List<IntegrationResponse>>> GetIntegrationByCompanyId(string companyId)
    {
        string uri = $"integrations/company/{companyId}";
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - InternalClient - Error trying to get the integration by externalId: {companyId}; apiKey: {apiKey}", companyId, apiKey);
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<ResponseWrapper<List<IntegrationResponse>>>())!;
    }

    public async Task<ResponseWrapper<OnlineStoresResponse>> OnlineStores(string[] merchantIds)
    {
        string uri = "stores/open";
        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(JsonSerializer.Serialize(merchantIds)));
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - InternalClient - Error trying to send merchantIds online: {merchantIds}", merchantIds.ToList());
            throw new Exception();
        }
        return (await response.Content.ReadFromJsonAsync<ResponseWrapper<OnlineStoresResponse>>())!;
    }
}
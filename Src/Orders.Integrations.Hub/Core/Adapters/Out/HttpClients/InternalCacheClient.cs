using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;

/// <summary>
/// This is a decorator cache class for IInternalClient, so it will try a cache hit before really calling the client inner method
/// </summary>
/// <param name="logger">The Logger</param>
/// <param name="cacheService">The cache service that will be injected</param>
/// <param name="inner">The inner method that will hit if there is a cache miss</param>
public class InternalCacheClient(
    ILogger<InternalClient> logger,
    ICacheService cacheService,
    IInternalClient inner
) : IInternalClient {
    public async Task<IntegrationResponse> GetIntegrationByExternalId(string externalId)
    {
        string cacheKey = $"integration:{externalId}";

        IntegrationResponse? integrationCached = cacheService.TryGet<IntegrationResponse>(cacheKey);

        if (integrationCached is not null)
            return integrationCached;

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("[INFO] - {context} - Getting integration by external id: {externalId}", nameof(InternalCacheClient), externalId);

        IntegrationResponse integration = await inner.GetIntegrationByExternalId(externalId);

        await cacheService.SetAsync(cacheKey, integration, TimeSpan.FromDays(1));

        return integration;
    }
}
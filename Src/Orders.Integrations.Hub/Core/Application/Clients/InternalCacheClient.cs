using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Internal;

namespace Orders.Integrations.Hub.Core.Application.Clients;

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

        logger.LogInformation("[INFO] - {context} - Getting integration by external id: {externalId}", nameof(InternalCacheClient), externalId);

        IntegrationResponse integration = await inner.GetIntegrationByExternalId(externalId);

        await cacheService.SetAsync(cacheKey, integration, TimeSpan.FromDays(1));

        return integration;
    }
}
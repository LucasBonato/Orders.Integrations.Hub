using Microsoft.Extensions.Caching.Hybrid;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

namespace Orders.Integrations.Hub.Core.Adapters.Out.Cache.Hybrid;

public sealed class HybridCacheService(
    HybridCache hybridCache
) : ICacheService {
    public async ValueTask<T?> GetAsync<T>(string key)
    {
        return await hybridCache.GetOrCreateAsync<T?>(
            key,
            _ => ValueTask.FromResult(default(T?))
        );
    }

    public async ValueTask SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        await hybridCache.SetAsync(
            key,
            value,
            new HybridCacheEntryOptions {
                Expiration = expiration,
                LocalCacheExpiration = expiration
            }
        );
    }
}
using Microsoft.Extensions.Caching.Memory;

namespace Orders.Integrations.Hub.Core.Adapters.Out.Cache.Memory;

public sealed class MemoryCacheService(
    IMemoryCache memoryCache
) : BaseCacheService {
    public override T? TryGet<T>(string key) where T : default
        => memoryCache.TryGetValue(key, out T? value) ? value : default;

    public override ValueTask SetAsync<T>(string key, T value, TimeSpan expiration) {
        memoryCache.Set(key, value, expiration);
        return ValueTask.CompletedTask;
    }
}
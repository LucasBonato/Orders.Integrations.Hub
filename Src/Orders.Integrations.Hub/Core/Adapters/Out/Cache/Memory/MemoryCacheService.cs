using Microsoft.Extensions.Caching.Memory;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

namespace Orders.Integrations.Hub.Core.Adapters.Out.Cache.Memory;

public sealed class MemoryCacheService(
    IMemoryCache memoryCache
) : ICacheService {
    public ValueTask<T?> GetAsync<T>(string key)
    {
        memoryCache.TryGetValue(key, out T? value);
        return ValueTask.FromResult(value);
    }

    public ValueTask SetAsync<T>(string key, T value, TimeSpan expiration) {
        memoryCache.Set(key, value, expiration);
        return ValueTask.CompletedTask;
    }
}
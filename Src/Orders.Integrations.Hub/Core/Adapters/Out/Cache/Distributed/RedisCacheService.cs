using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

namespace Orders.Integrations.Hub.Core.Adapters.Out.Cache.Distributed;

public sealed class RedisCacheService(
    IDistributedCache distributedCache
) : ICacheService {
    private static readonly JsonSerializerOptions JsonOptions =  new(JsonSerializerDefaults.Web);
    
    public async ValueTask<T?> GetAsync<T>(string key) {
        byte[]? data = await distributedCache.GetAsync(key);
        
        if (data is null || data.Length == 0)
            return default;
        
        return JsonSerializer.Deserialize<T>(data, JsonOptions);
    }

    public async ValueTask SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        await distributedCache.SetAsync(
            key, 
            JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions), 
            new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = expiration
            }
        );
    }
}
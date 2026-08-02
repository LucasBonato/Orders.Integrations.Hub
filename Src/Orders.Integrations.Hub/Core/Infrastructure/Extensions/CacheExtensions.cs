using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Distributed;
using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Hybrid;
using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Memory;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddCacheConfiguration(this IServiceCollection services) {
        string cacheMode = AppEnv.CACHE.MODE.NotNullEnv();

        return cacheMode switch {
            "Memory" => services
                .AddMemoryCache()
                .AddSingleton<ICacheService, MemoryCacheService>(),
                
            "Distributed" => services
                .AddStackExchangeRedisCache(options => {
                    options.Configuration = AppEnv.CACHE.CONFIGURATIONS.CONNECTION_STRING.NotNullEnv();
                    options.InstanceName = "redis-only-instance";
                })
                .AddSingleton<ICacheService, RedisCacheService>(),
                
            "Hybrid" => services
                .AddMemoryCache()
                .AddStackExchangeRedisCache(options => {
                    options.Configuration = AppEnv.CACHE.CONFIGURATIONS.CONNECTION_STRING.NotNullEnv();
                    options.InstanceName = "redis-hybrid-instance";
                })
                .AddHybridCache()
                .Services
                .AddSingleton<ICacheService, HybridCacheService>(),
                
            _ => throw new InvalidOperationException("Invalid cache mode!")
        };
    }
}
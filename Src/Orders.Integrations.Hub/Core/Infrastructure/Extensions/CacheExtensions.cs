using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Distributed;
using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Hybrid;
using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Memory;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Infrastructure.Options;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, IConfiguration configuration) {
        
        CacheOptions cacheOptions = configuration
            .GetSection(CacheOptions.SectionName)
            .Get<CacheOptions>()
                ?? throw new InvalidOperationException($"Missing '{CacheOptions.SectionName}' configuration section.");
        
        string connectionString = cacheOptions.Provider == CacheProvider.Memory
            ? string.Empty
            : configuration.GetConnectionString("Redis")
                ?? throw new InvalidOperationException("Missing 'ConnectionStrings:Redis' configuration value.");
        
        return cacheOptions.Provider switch {
            CacheProvider.Memory => services
                .AddMemoryCache()
                .AddSingleton<ICacheService, MemoryCacheService>(),
                
            CacheProvider.Distributed => services
                .AddStackExchangeRedisCache(options => {
                    options.Configuration = connectionString;
                    options.InstanceName = "redis-only-instance";
                })
                .AddSingleton<ICacheService, RedisCacheService>(),
                
            CacheProvider.Hybrid => services
                .AddMemoryCache()
                .AddStackExchangeRedisCache(options => {
                    options.Configuration = connectionString;
                    options.InstanceName = "redis-hybrid-instance";
                })
                .AddHybridCache()
                .Services
                .AddSingleton<ICacheService, HybridCacheService>(),
                
            _ => throw new InvalidOperationException("Invalid cache mode!")
        };
    }
}
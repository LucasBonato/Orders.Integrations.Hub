using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

namespace Orders.Integrations.Hub.Integrations.Common.Extensions;

public static class CacheServiceExtensions
{
    public static async ValueTask<string> GetOrSetTokenAsync(
        this ICacheService cacheService,
        string key,
        string contextName,
        ILogger logger,
        Func<Task<(string AccessToken, TimeSpan Expiration)>> tokenFallback
    ) {
        string? accessToken = await cacheService.GetAsync<string>(key);

        if (!string.IsNullOrEmpty(accessToken))
            return accessToken;

        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning("[WARN] - {context} - Generating new token", contextName);

        (string newAccessToken, TimeSpan newExpiration) = await tokenFallback();

        if (string.IsNullOrEmpty(newAccessToken))
            throw new InvalidOperationException("Could not generate new token");

        await cacheService.SetAsync(key, newAccessToken, newExpiration);

        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning("[WARN] - {context} - Adding token to cache", contextName);

        return newAccessToken;
    }
}
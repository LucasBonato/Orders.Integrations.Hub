using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

namespace Orders.Integrations.Hub.Core.Adapters.Out.Cache;

public abstract class BaseCacheService : ICacheService
{
    public abstract T? TryGet<T>(string key);

    public abstract ValueTask SetAsync<T>(string key, T value, TimeSpan expiration);

    public async ValueTask<string> GetOrSetTokenAsync(
        string key,
        string contextName,
        ILogger logger,
        Func<Task<(string AccessToken, TimeSpan Expiration)>> tokenFallback
    ) {
        string? accessToken = TryGet<string>(key);

        if (!string.IsNullOrEmpty(accessToken))
            return accessToken;

        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning("[WARN] - {context} - Generating new token", contextName);

        (string newAccessToken, TimeSpan newExpiration) = await tokenFallback().ConfigureAwait(false);

        if (string.IsNullOrEmpty(newAccessToken))
            throw new InvalidOperationException("Could not generate new token");

        await SetAsync(key, newAccessToken, newExpiration).ConfigureAwait(false);

        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning("[WARN] - {context} - Adding token to cache", contextName);

        return newAccessToken;
    }
}
namespace Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

public interface ICacheService {
    T? TryGet<T>(string key);
    ValueTask SetAsync<T>(string key, T value, TimeSpan expiration);
    ValueTask<string> GetOrSetTokenAsync(
        string key,
        string contextName,
        ILogger logger,
        Func<Task<(string AccessToken, TimeSpan Expiration)>> tokenFallback
    );
}
namespace Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

public interface ICacheService {
    ValueTask<T?> GetAsync<T>(string key);
    ValueTask SetAsync<T>(string key, T value, TimeSpan expiration);
}
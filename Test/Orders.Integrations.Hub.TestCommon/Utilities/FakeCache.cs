using System.Collections.Concurrent;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;

namespace Orders.Integrations.Hub.TestCommon.Utilities;

public sealed class FakeCache : ICacheService
{
    private readonly ConcurrentDictionary<string, (object? Value, DateTime ExpiresAt)> _store = new();

    public ValueTask<T?> GetAsync<T>(string key) {
        if (!_store.TryGetValue(key, out (object? Value, DateTime ExpiresAt) entry))
            return ValueTask.FromResult<T?>(default);

        if (DateTime.UtcNow < entry.ExpiresAt)
            return ValueTask.FromResult((T?)entry.Value);

        _store.TryRemove(key, out _);

        return ValueTask.FromResult<T?>(default);
    }

    public ValueTask SetAsync<T>(string key, T value, TimeSpan expiration) {
        _store[key] = (value, DateTime.UtcNow.Add(expiration));
        return ValueTask.CompletedTask;
    }
}
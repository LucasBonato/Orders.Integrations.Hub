# Cache Tests

The `ICacheService` interface has three implementations: `MemoryCacheService`, `RedisCacheService`, and `HybridCacheService`. Tests validate that all three behave identically under the contract.

## ICacheService Contract

```csharp
public interface ICacheService
{
    ValueTask<T?> GetAsync<T>(string key);
    ValueTask SetAsync<T>(string key, T value, TimeSpan expiration);
}
```

Every implementation must support: get, set, overwrite, expiration, complex types, null returns, and nullable value types.

## Shared Test Patterns

### Get/Set/Expire

```csharp
[Fact]
public async Task GetAsync_ShouldReturnDefault_WhenKeyDoesNotExist()
{
    string? result = await _sut.GetAsync<string>("missing-key");
    Assert.Null(result);
}

[Fact]
public async Task SetAsync_ShouldStoreValue_RetrievableByGet()
{
    await _sut.SetAsync("key", "stored-value", TimeSpan.FromMinutes(1));
    string? result = await _sut.GetAsync<string>("key");
    Assert.Equal("stored-value", result);
}
```

### Expiration (short TTLs)

```csharp
[Fact]
public async Task GetAsync_ShouldReturnDefault_AfterExpiration()
{
    await _sut.SetAsync("expiring", "value", TimeSpan.FromMilliseconds(50));
    await Task.Delay(100);
    string? result = await _sut.GetAsync<string>("expiring");
    Assert.Null(result);
}
```

**Note**: `HybridCacheService` needs ~150ms delay due to internal write-behind.

### Complex Types and Overwrite

```csharp
[Fact]
public async Task SetAsync_ShouldStoreComplexType()
{
    var record = new CachedRecord("hello", 42);
    await _sut.SetAsync("record-key", record, TimeSpan.FromMinutes(1));
    CachedRecord? result = await _sut.GetAsync<CachedRecord>("record-key");
    Assert.NotNull(result);
    Assert.Equal("hello", result.Name);
}

[Fact]
public async Task SetAsync_ShouldOverwriteExistingKey()
{
    await _sut.SetAsync("key", "first", TimeSpan.FromMinutes(1));
    await _sut.SetAsync("key", "second", TimeSpan.FromMinutes(1));
    string? result = await _sut.GetAsync<string>("key");
    Assert.Equal("second", result);
}
```

## Implementation Differences

| Implementation | Setup | Notes |
|---|---|---|
| **MemoryCacheService** | `new MemoryCacheService(new MemoryCache(...))` | Fastest, no deps |
| **RedisCacheService** | `new RedisCacheService(new MemoryDistributedCache(...))` | Tests JSON serialization layer |
| **HybridCacheService** | Requires `AddHybridCache()` DI setup | Tests L1+L2 cache coordination |

## FakeCache for Unit Tests

`FakeCache` from TestCommon stores values with TTL without external infrastructure:

```csharp
var cache = new FakeCache(TimeSpan.FromMinutes(5));
await cache.SetAsync("key", "value", TimeSpan.FromSeconds(30));
string? result = await cache.GetAsync<string>("key");
```

## Redis with TestContainers

For tests that must verify behavior with a real Redis instance:

```csharp
[Collection("Redis")]
public class RedisIntegrationTests
{
    private readonly RedisContainer _redis;
    public RedisIntegrationTests(RedisContainerFixture fixture) => _redis = fixture.Container;
}
```

## TTL and Expiration Guidelines

- Unit tests: 50ms TTL + 100ms delay
- Integration tests: 1-2s TTL
- Hybrid cache: allow extra time for L1→L2 write-behind
- **Never use delays >5s** — slow suite indicates design problem
- Test expired values are not returned (not just disappearance)
- `CacheServiceExtensionsTests` validate `GetOrSetTokenAsync` helper combining cache + auth retrieval

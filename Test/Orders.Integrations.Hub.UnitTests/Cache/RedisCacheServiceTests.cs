using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Distributed;

namespace Orders.Integrations.Hub.UnitTests.Cache;

public class RedisCacheServiceTests
{
    private static IDistributedCache BuildDistributedCache() =>
        new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
    
    private readonly RedisCacheService _sut = new(BuildDistributedCache());

    [Fact]
    public async Task GetAsync_ShouldReturnDefault_WhenKeyDoesNotExist()
    {
        string? result = await _sut.GetAsync<string>("missing");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnStoredValue()
    {
        await _sut.SetAsync("key", "value", TimeSpan.FromMinutes(1));

        string? result = await _sut.GetAsync<string>("key");

        Assert.Equal("value", result);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDefault_AfterExpiration()
    {
        await _sut.SetAsync("key", "value", TimeSpan.FromMilliseconds(50));

        await Task.Delay(100, TestContext.Current.CancellationToken);

        string? result = await _sut.GetAsync<string>("key");
        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_ShouldStoreComplexType_AndDeserializeCorrectly()
    {
        var record = new CachedRecord("redis-test", 99);

        await _sut.SetAsync("record", record, TimeSpan.FromMinutes(1));
        CachedRecord? result = await _sut.GetAsync<CachedRecord>("record");

        Assert.NotNull(result);
        Assert.Equal("redis-test", result.Name);
        Assert.Equal(99, result.Value);
    }

    [Fact]
    public async Task SetAsync_ShouldOverwriteExistingKey()
    {
        await _sut.SetAsync("key", "first", TimeSpan.FromMinutes(1));
        await _sut.SetAsync("key", "second", TimeSpan.FromMinutes(1));

        string? result = await _sut.GetAsync<string>("key");

        Assert.Equal("second", result);
    }

    [Fact]
    public async Task GetAsync_ShouldHandleNullableValueType()
    {
        int? result = await _sut.GetAsync<int?>("missing");

        Assert.Null(result);
    }
}
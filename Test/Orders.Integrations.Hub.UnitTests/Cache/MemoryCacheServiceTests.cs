using Microsoft.Extensions.Caching.Memory;

using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Memory;

namespace Orders.Integrations.Hub.UnitTests.Cache;

public class MemoryCacheServiceTests: IDisposable {

    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheService _sut;
    
    public MemoryCacheServiceTests()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _sut = new MemoryCacheService(_memoryCache);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnDefault_WhenKeyDoesNotExist()
    {
        string? result = await _sut.GetAsync<string>("missing-key");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnValue_WhenKeyExists()
    {
        _memoryCache.Set("key", "value", TimeSpan.FromMinutes(1));

        string? result = await _sut.GetAsync<string>("key");

        Assert.Equal("value", result);
    }

    [Fact]
    public async Task SetAsync_ShouldStoreValue_RetrievableByGet()
    {
        await _sut.SetAsync("key", "stored-value", TimeSpan.FromMinutes(1));

        string? result = await _sut.GetAsync<string>("key");

        Assert.Equal("stored-value", result);
    }

    [Fact]
    public async Task SetAsync_ShouldStoreComplexType()
    {
        var record = new CachedRecord("hello", 42);
        await _sut.SetAsync("record-key", record, TimeSpan.FromMinutes(1));

        CachedRecord? result = await _sut.GetAsync<CachedRecord>("record-key");

        Assert.NotNull(result);
        Assert.Equal("hello", result.Name);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDefault_AfterExpiration()
    {
        await _sut.SetAsync("expiring", "value", TimeSpan.FromMilliseconds(50));

        await Task.Delay(100, TestContext.Current.CancellationToken);

        string? result = await _sut.GetAsync<string>("expiring");
        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_ShouldOverwriteExistingKey()
    {
        await _sut.SetAsync("key", "first", TimeSpan.FromMinutes(1));
        await _sut.SetAsync("key", "second", TimeSpan.FromMinutes(1));

        string? result = await _sut.GetAsync<string>("key");

        Assert.Equal("second", result);
    }
    
    public void Dispose() => _memoryCache.Dispose();
}
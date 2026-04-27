using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Hybrid;

namespace Orders.Integrations.Hub.UnitTests.Cache;

public class HybridCacheServiceTests
{
    private readonly HybridCacheService _sut;

    public HybridCacheServiceTests()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddMemoryCache()
            .AddHybridCache()
            .Services
            .BuildServiceProvider();

        HybridCache hybridCache = provider.GetRequiredService<HybridCache>();
        _sut = new HybridCacheService(hybridCache);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDefault_WhenKeyDoesNotExist()
    {
        string? result = await _sut.GetAsync<string>("missing-key");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnStoredValue_AfterSet()
    {
        await _sut.SetAsync("key", "value", TimeSpan.FromMinutes(1));

        string? result = await _sut.GetAsync<string>("key");

        Assert.Equal("value", result);
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
    public async Task SetAsync_ShouldStoreComplexType_AndRetrieveCorrectly()
    {
        CachedRecord record = new("hybrid", 7);

        await _sut.SetAsync("record", record, TimeSpan.FromMinutes(1));
        CachedRecord? result = await _sut.GetAsync<CachedRecord>("record");

        Assert.NotNull(result);
        Assert.Equal("hybrid", result.Name);
        Assert.Equal(7, result.Value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDefault_AfterExpiration()
    {
        await _sut.SetAsync("key", "value", TimeSpan.FromMilliseconds(50));

        await Task.Delay(150, TestContext.Current.CancellationToken);

        string? result = await _sut.GetAsync<string>("key");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldHandleNullableValueType()
    {
        int? result = await _sut.GetAsync<int?>("missing");

        Assert.Null(result);
    }
}
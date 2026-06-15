using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Memory;
using Orders.Integrations.Hub.Integrations.Common.Extensions;

namespace Orders.Integrations.Hub.UnitTests.Cache;

public class CacheServiceExtensionsTests
{
    private readonly ILogger _logger = NullLogger.Instance;

    // Uses MemoryCacheService as a real implementation — no need to mock ICacheService here
    private static MemoryCacheService BuildCache() =>
        new(new MemoryCache(new MemoryCacheOptions()));

    [Fact]
    public async Task GetOrSetTokenAsync_ShouldCallFallback_WhenCacheMiss()
    {
        var cache = BuildCache();
        int callCount = 0;

        await cache.GetOrSetTokenAsync(
            "token-key",
            "TestContext",
            _logger,
            () => {
                callCount++;
                return Task.FromResult(("new-token", TimeSpan.FromMinutes(5)));
            }
        );

        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task GetOrSetTokenAsync_ShouldReturnCachedToken_WithoutCallingFallback()
    {
        var cache = BuildCache();
        await cache.SetAsync("token-key", "existing-token", TimeSpan.FromMinutes(5));

        int callCount = 0;
        string result = await cache.GetOrSetTokenAsync(
            "token-key",
            "TestContext",
            _logger,
            () => {
                callCount++;
                return Task.FromResult(("new-token", TimeSpan.FromMinutes(5)));
            }
        );

        Assert.Equal("existing-token", result);
        Assert.Equal(0, callCount);
    }

    [Fact]
    public async Task GetOrSetTokenAsync_ShouldCacheToken_ReturnedByFallback()
    {
        var cache = BuildCache();

        string result = await cache.GetOrSetTokenAsync(
            "token-key",
            "TestContext",
            _logger,
            () => Task.FromResult(("cached-token", TimeSpan.FromMinutes(5)))
        );

        Assert.Equal("cached-token", result);

        // Second call must NOT invoke fallback
        int secondCallCount = 0;
        string secondResult = await cache.GetOrSetTokenAsync(
            "token-key",
            "TestContext",
            _logger,
            () => {
                secondCallCount++;
                return Task.FromResult(("should-not-be-called", TimeSpan.FromMinutes(5)));
            }
        );

        Assert.Equal("cached-token", secondResult);
        Assert.Equal(0, secondCallCount);
    }

    [Fact]
    public async Task GetOrSetTokenAsync_ShouldThrow_WhenFallbackReturnsEmptyToken()
    {
        var cache = BuildCache();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            cache.GetOrSetTokenAsync(
                "token-key",
                "TestContext",
                _logger,
                () => Task.FromResult((string.Empty, TimeSpan.FromMinutes(5)))
            ).AsTask()
        );
    }

    [Fact]
    public async Task GetOrSetTokenAsync_ShouldPropagate_WhenFallbackThrows()
    {
        var cache = BuildCache();

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            cache.GetOrSetTokenAsync(
                "token-key",
                "TestContext",
                _logger,
                () => throw new HttpRequestException("Auth service down")
            ).AsTask()
        );
    }

    [Fact]
    public async Task GetOrSetTokenAsync_ShouldNotCacheToken_WhenFallbackThrows()
    {
        var cache = BuildCache();

        try
        {
            await cache.GetOrSetTokenAsync(
                "token-key",
                "TestContext",
                _logger,
                () => throw new HttpRequestException("Auth service down")
            ).AsTask();
        }
        catch { /* expected */ }

        // Cache should be empty — next call should invoke fallback again
        int callCount = 0;
        await cache.GetOrSetTokenAsync(
            "token-key",
            "TestContext",
            _logger,
            () => {
                callCount++;
                return Task.FromResult(("recovered-token", TimeSpan.FromMinutes(5)));
            }
        );

        Assert.Equal(1, callCount);
    }
}
# Common Pitfalls

## Integration Namespace Collision with `Integration` Type

The `Orders.Integrations.Hub.Integrations.Common.ValueObjects` namespace contains a type named `Integration`. This collides with the `Orders.Integrations.Hub.Integrations` namespace. Always use a `using` alias:

```csharp
using IntegrationRecord = Orders.Integrations.Hub.Integrations.Common.ValueObjects.Integration;
```

Tests without this alias get ambiguous reference compile errors. See `AuthHandlerTestFixture.cs` for the correct pattern.

## Task.Delay for Expiration Tests

Avoid `Task.Delay` where possible. When testing cache expiration:

- Use **short TTLs (50ms)** with matching delays
- For `HybridCacheService`, allow **150ms** due to internal write-behind
- Never use delays >5s — they slow the suite and indicate a design problem
- Prefer `FakeCache` for unit tests (it simulates expiration with wall-clock comparison)

## Environment Variable Leaks

Tests that set environment variables must clean up in `Dispose`:

```csharp
public void Dispose()
{
    Environment.SetEnvironmentVariable("MY_VAR", null);
}
```

Failing to clean up causes non-deterministic test failures when tests run in parallel.

## Over-Mocking Leads to Fragile Tests

**Don't mock what you can use:**

| Instead of mocking `ICacheService` | Use `FakeCache` |
|---|---|
| Instead of mocking `IObjectStorageClient` | Use `FakeStorage` |
| Instead of mocking `IMemoryCache` | Use `new MemoryCache(new MemoryCacheOptions())` |
| Instead of mocking `IDistributedCache` | Use `new MemoryDistributedCache(...)` |

Real implementations catch more bugs and reduce test maintenance.

## Magic Values: Use Constants

```csharp
// BAD
var integration = new Integration("tenant-1", "merchant-1", ...);

// GOOD
private const string TestTenantId = "tenant-1";
var integration = ObjectMother.CreateIntegration();
```

## Test Order Dependencies

Every test must be independent and parallelizable. Common violations:

- Modifying shared static state (environment variables, `AsyncLocal`)
- Depending on test execution order
- Using `[Collection]` when not needed (only for shared infrastructure)
- Forgetting `IAsyncLifetime.InitializeAsync` calls `_harness.Start()`

## Missing TestCommon Reference in New Test Projects

New test projects must add:

```xml
<ItemGroup>
    <ProjectReference Include="..\Orders.Integrations.Hub.TestCommon\Orders.Integrations.Hub.TestCommon.csproj" />
</ItemGroup>
```

Without this, builders, fakes, and fixtures are unavailable.

## Async Void in Tests

```csharp
// BAD — exceptions disappear
[Fact]
public async void MyTest() { ... }

// GOOD
[Fact]
public async Task MyTest() { ... }
```

## Disposal of ITestHarness

Always implement `IAsyncLifetime` and call `await _harness.Stop()` in `DisposeAsync`. Failing to stop the harness can leave background tasks running:

```csharp
public async ValueTask DisposeAsync()
{
    await _harness.Stop();
    GC.SuppressFinalize(this);
}
```

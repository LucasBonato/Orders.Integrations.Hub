# Fixtures and Utilities

TestCommon at `Test/Orders.Integrations.Hub.TestCommon/` provides shared fixtures, fakes, and utilities used across unit and integration test projects.

## AuthHandlerTestFixture (Polymorphic Pattern)

The `AuthHandlerTestFixture` abstract class enables testing auth handlers across **all integrations** with the same shared test methods using xUnit's `ClassData`:

```csharp
public abstract class AuthHandlerTestFixture : IXunitSerializable
{
    public string Name { get; set; } = string.Empty;
    public abstract TestHandler InnerHandler { get; }
    public abstract DelegatingHandler Handler { get; }
    public abstract IIntegrationContext CreateContext();
    public abstract void AssertAuthHeader(HttpRequestMessage request, string expectedToken);
    public abstract void SetupCacheMiss();
    public abstract void SetupCacheHit(string token);
    public abstract void SetupAuthSuccess(string token, TimeSpan expiration);
    public abstract void SetupAuthFailure(Exception exception);
}
```

Each integration provides a concrete implementation and registers it in `AuthHandlerFixtureProvider`:

```csharp
public class AuthHandlerFixtureProvider : TheoryData<AuthHandlerTestFixture>
{
    public AuthHandlerFixtureProvider()
    {
        Add(new IFoodAuthHandlerFixture("IFood"));
        Add(new RappiAuthHandlerFixture("Rappi"));
        Add(new Food99AuthHandlerFixture("Food99"));
    }
}
```

Test methods in `AuthMessageHandlerTests` use `[Theory] [ClassData(...)]` to run the same assertions for every integration.

## FakeCache

Replaces `ICacheService` for unit tests. Uses `ConcurrentDictionary` with TTL-aware expiration:

```csharp
public sealed class FakeCache : ICacheService
{
    public ValueTask<T?> GetAsync<T>(string key) { /* checks expiration */ }
    public ValueTask SetAsync<T>(string key, T value, TimeSpan expiration) { /* stores with expiry */ }
    public void Clear() => _store.Clear();
    public int Count => _store.Count;
}
```

Use instead of `Substitute.For<ICacheService>()` when testing code that calls both Get and Set.

## FakeStorage

Replaces `IObjectStorageClient` for unit tests:

```csharp
public sealed class FakeStorage : IObjectStorageClient
{
    public Task<string> UploadFile(Stream file, string contentType, string key) { /* stores bytes */ }
    public Task DeleteFile(string key) { /* removes key */ }
    public Task DeleteFolder(string pathKey) { /* removes all with prefix */ }
    public string GetTemporaryUrl(string key, TimeSpan? expiry = null) { /* returns fake URL */ }
    public bool Exists(string key) => _store.ContainsKey(key);
    public int Count => _store.Count;
}
```

## ObjectMother

Static factory for commonly needed objects:

```csharp
public static class ObjectMother
{
    public static Integration CreateIntegration(...);
    public static IIntegrationContext CreateIntegrationContext(...);
}
```

Use when you need a quick, deterministic object without builder overhead.

## Collection Fixtures for TestContainers

Expensive containers (Redis, LocalStack, RabbitMQ) use collection fixtures:

```csharp
[CollectionDefinition("Redis")]
public class RedisCollection : ICollectionFixture<RedisContainerFixture> { }

public class RedisContainerFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        Container = new RedisBuilder().WithImage("redis:7-alpine").Build();
        await Container.StartAsync();
    }
    public async Task DisposeAsync() => await Container.DisposeAsync();
}
```

## TestCommon Contents

| File | Purpose |
|---|---|
| `Fakers/Commands/*` | Bogus builders for command objects |
| `Fakers/Order/*` | Bogus builders for domain entities |
| `Fixtures/ObjectMother.cs` | Static factory for `Integration` and `IIntegrationContext` |
| `Utilities/FakeCache.cs` | In-memory `ICacheService` with TTL |
| `Utilities/FakeStorage.cs` | In-memory `IObjectStorageClient` |

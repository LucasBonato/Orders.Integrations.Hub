# Unit Testing

Unit tests in this repo are **fast, isolated, and deterministic**. They test one class in isolation by mocking its dependencies and exercising every code path.

## What Makes a Good Unit Test

- **One behavior per test** — see `IntegrationRouterTests.Resolve_ShouldReturnService_WhenKeyedServiceRegistered`
- **AAA structure** — Arrange, Act, Assert separated explicitly
- **No I/O** — no database, no HTTP, no filesystem. Use `NSubstitute` for external calls
- **No Task.Delay** — use short TTLs (50ms) if expiration tests are needed, but prefer real cache implementations with `MemoryCache` instead

## Patterns

### Serializers (`ICustomJsonSerializer`)

```csharp
// From CoreJsonSerializerTests
[Fact]
public void Serialize_ShouldUseCamelCase()
{
    string json = Sut.Serialize(new { SomeProperty = "value" });
    Assert.Contains("someProperty", json);
}

[Fact]
public void Serialize_ShouldUseSnakeCaseUpperForEnums()
{
    string json = Sut.Serialize(new { Status = TestEnum.FirstValue });
    Assert.Contains("FIRST_VALUE", json);
}

[Fact]
public void RoundTrip_ShouldPreserveValues()
{
    var original = new TestDto("roundtrip", 99);
    string json = Sut.Serialize(original);
    var result = Sut.Deserialize<TestDto>(json);
    Assert.NotNull(result);
    Assert.Equal(original.Name, result.Name);
    Assert.Equal(original.Count, result.Count);
}

[Fact]
public void Deserialize_ShouldReturnDefault_WhenNull()
{
    var result = Sut.Deserialize<TestDto>("null");
    Assert.Null(result);
}
```

Each integration has a serializer test class following this exact pattern but with the integration's naming conventions (e.g., `RappiJsonSerializerTests` tests snake_case lower + `UPPER_SNAKE_CASE` enums; `Food99JsonSerializerTests` tests snake_case lower + camelCase enums).

### Mapping Extensions with Real Payloads

Test mapping by deserializing payload fixtures and asserting every mapped field on the domain object:

```csharp
[Fact]
public void Map_ShouldConvertAllFields()
{
    string json = File.ReadAllText("Fixtures/rappi-webhook-create-order.json");
    var dto = new RappiJsonSerializer().Deserialize<RappiWebhookEventOrderRequest>(json);

    Order order = dto.MapToOrder();

    Assert.Equal("ext-order-123", order.OrderId);
    Assert.Equal(OrderType.DELIVERY, order.Type);
    Assert.Equal("RAPPI", order.SalesChannel);
}
```

### IntegrationRouter (Keyed DI)

```csharp
[Fact]
public void Resolve_ShouldReturnService_WhenKeyedServiceRegistered()
{
    var services = new ServiceCollection();
    services.AddKeyedScoped<ITestUseCase, TestUseCaseImpl>(TestKey);
    ServiceProvider provider = services.BuildServiceProvider();
    var router = new IntegrationRouter(provider);

    ITestUseCase useCase = router.Resolve<ITestUseCase>(IntegrationKey.From(TestKey));

    Assert.NotNull(useCase);
}

[Fact]
public void Resolve_ShouldThrow_WhenServiceNotRegistered()
{
    var services = new ServiceCollection();
    ServiceProvider provider = services.BuildServiceProvider();
    var router = new IntegrationRouter(provider);

    Assert.Throws<UnknownIntegrationException>(() =>
        router.Resolve<ITestUseCase>(IntegrationKey.From("MISSING")));
}
```

Test `Resolve`, `CanResolve` (true/false), and scoped lifetime behavior.

### Validators

```csharp
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("ifood")]
[InlineData("IFOOD ")]
public void ValidateRawValue_ShouldThrow_WhenInvalid(string value)
{
    Assert.Throws<InvalidOperationException>(() =>
        IntegrationKeyValidator.ValidateRawValue(value));
}

[Fact]
public void ValidateRawValue_ShouldNotThrow_WhenValid()
{
    var exception = Record.Exception(() =>
        IntegrationKeyValidator.ValidateRawValue("IFOOD"));
    Assert.Null(exception);
}
```

### Middleware

Test exception handler middleware by substituting `IProblemDetailsService` and asserting the correct status code for each exception type. See `ExceptionHandlerMiddlewareTests` for the pattern.

## Mocking Guidelines with NSubstitute

- **Substitute.For\<T>()** — create mocks for interfaces
- **Returns()** — set return values
- **Received(1)** / **DidNotReceiveWithAnyArgs()** — verify calls (preferred over `Received()`)
- **Throws\<T>()** / **ThrowsAsync\<T>()** — simulate failures
- **Arg.Is\<T>(predicate)** — match specific arguments
- **Arg.Any\<T>()** — accept any argument
- **Do not mock ICustomJsonSerializer** in handler tests — use a real serializer
- **Do not mock ICacheService** in auth handler tests — use `FakeCache` from TestCommon

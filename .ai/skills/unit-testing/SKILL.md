# unit-testing

**When:** Creating or modifying unit tests for serializers, handlers, routers, validators, middleware, or any pure logic.

## Patterns

- **AAA:** Arrange → Act → Assert as explicit structural separation (see `AuthMessageHandlerTests`, `IntegrationKeyValidatorTests`)
- **One behavior per test:** One `[Fact]` or `[Theory]`, one `Act`, one set of `Assert` calls on a single outcome
- **NSubstitute** for mocking interfaces: `Substitute.For<T>()`, `Arg.Is<>()`, `.Received(1)`, `.DidNotReceiveWithAnyArgs()`,
- **Real implementations** for infrastructure when practical (e.g., `MemoryDistributedCache` for `RedisCacheServiceTests`, `MemoryCache` for `MemoryCacheServiceTests`)
- **`[Theory]` + `[ClassData]`** for polymorphic tests across integrations (see `AuthMessageHandlerTests` + `AuthHandlerFixtureProvider`)
- **`Record.Exception()`** for asserting no exception thrown

## Examples

| Test Class | File Path | What It Tests |
|---|---|---|
| `CoreJsonSerializerTests` | `Test/.../Serialization/CoreJsonSerializerTests.cs` | camelCase, snake_case enums, round-trip, null |
| `Food99JsonSerializerTests` | `Test/.../Serialization/Food99JsonSerializerTests.cs` | snake_case lower, camelCase enums, round-trip |
| `RappiJsonSerializerTests` | `Test/.../Serialization/RappiJsonSerializerTests.cs` | snake_case lower, snake_case upper enums |
| `IntegrationRouterTests` | `Test/.../Integration/IntegrationRouterTests.cs` | keyed DI resolution, scoped lifetime, missing key |
| `EnumBasedCancellationReasonUseCaseTests` | `Test/.../Integration/EnumBasedCancellationReasonUseCaseTests.cs` | enum mapping to DTOs, empty enum, ID invariance |
| `IntegrationKeyValidatorTests` | `Test/.../Integration/IntegrationKeyValidatorTests.cs` | null, whitespace, casing, valid values |
| `IntegrationKeyValidationTests` | `Test/.../Integrations/IntegrationKeyValidationTests.cs` | reflection-based attribute/field validation |
| `HybridCacheServiceTests` | `Test/.../Cache/HybridCacheServiceTests.cs` | get/set, expiration, complex types, overwrite |
| `ExceptionHandlerMiddlewareTests` | `Test/.../Middleware/ExceptionHandlerMiddlewareTests.cs` | problem details, status codes, custom exceptions |
| `IntegrationPipelineTests` | `Test/.../Pipeline/IntegrationPipelineTests.cs` | full auth pipeline per integration |
| `IntegrationContextHandlerTests` | `Test/.../Handlers/IntegrationContextHandlerTests.cs` | context injection, null HTTP context, delegation |

## Conventions

- **File placement mirrors source namespace:**
  - Source: `Src/.../Core/Infrastructure/Serialization/CoreJsonSerializer.cs`
  - Test: `Test/.../UnitTests/Serialization/CoreJsonSerializerTests.cs`
- **Test class naming:** `{ClassName}Tests`
- **Method naming:** `{Method}_Should_{ExpectedBehavior}_When_{Condition}` (e.g., `SendAsync_ShouldSkipAuth_WhenAuthorizationHeaderAlreadySet`)
- **SUT naming:** `private static readonly {Type} Sut = new();` for stateless, `private readonly {Type} _sut;` for stateful
- **Use `TestContext.Current.CancellationToken`** instead of `CancellationToken.None` (xUnit v3 pattern)
- **Use `TestCommon` Fakers** (`new CreateOrderCommandFaker().Generate()`) for complex domain objects
- **Use `TestCommon/Fixtures/ObjectMother`** for pre-built common objects like `Integration` and `IIntegrationContext`
- **`using` statements inside namespace** (not outside) per project convention

## Anti-Patterns

- ❌ **Over-mocking:** Don't mock `ICacheService` when `MemoryCache` works as a real in-memory impl (see `CacheServiceExtensionsTests`)
- ❌ **Sleeping/`Task.Delay` with arbitrary waits:** Use `TaskCompletionSource` or short + deterministic delays if unavoidable
- ❌ **Testing implementation details:** Test public behavior (serialize → deserialize → assert values), not internal method calls
- ❌ **Multiple asserts for different behaviors:** If you need to assert "value is X" and "was called once", that's two behaviors — use two tests
- ❌ **Using real RabbitMQ/Redis/HTTP in unit tests:** Mock external boundaries; use `TestContainers` only in integration tests
- ❌ **Mixing AAA blocks:** Don't interleave Arrange and Act — clear structural separation

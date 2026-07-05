# fixtures

**When:** Creating test fixtures for shared setup — polymorphic test data per integration, concurrent-safe fakes, container lifecycle management.

## Patterns

- **Polymorphic fixtures** via abstract `AuthHandlerTestFixture` — one fixture per integration, shared test methods via `[ClassData]`
- **Collection fixtures** for container-backed tests (`IAsyncLifetime` + `TestContainers.Redis`)
- **ObjectMother** for simple pre-built objects with optional overrides (see `TestCommon/Fixtures/ObjectMother.cs`)
- **Fakes** (`FakeCache`, `FakeStorage`) implement port interfaces directly — no mocking framework needed

## Examples

| Fixture | File Path | What It Provides |
|---|---|---|
| `AuthHandlerTestFixture` | `Test/.../UnitTests/Handlers/AuthHandlerTestFixture.cs` | Abstract base with `CreateContext()`, `SetupCache*()`, `SetupAuth*()`, `AssertAuthHeader()` |
| `IFoodAuthHandlerFixture` | `Test/.../UnitTests/Handlers/Fixtures/IFoodAuthHandlerFixture.cs` | Concrete IFood auth handler with mocked `IIFoodAuthClient` |
| `RappiAuthHandlerFixture` | `Test/.../UnitTests/Handlers/Fixtures/RappiAuthHandlerFixture.cs` | Concrete Rappi auth handler with mocked `IRappiAuthClient` |
| `Food99AuthHandlerFixture` | `Test/.../UnitTests/Handlers/Fixtures/Food99AuthHandlerFixture.cs` | Concrete Food99 auth handler with mocked `IFood99AuthClient` |
| `AuthHandlerFixtureProvider` | `Test/.../UnitTests/Handlers/AuthHandlerFixtureProvider.cs` | `TheoryData<AuthHandlerTestFixture>` for `[ClassData]` |
| `ObjectMother` | `TestCommon/Fixtures/ObjectMother.cs` | `CreateIntegration()`, `CreateIntegrationContext()` |
| `FakeCache` | `TestCommon/Utilities/FakeCache.cs` | In-memory `ICacheService` with TTL |
| `FakeStorage` | `TestCommon/Utilities/FakeStorage.cs` | In-memory `IObjectStorageClient` |
| `TestHandler` | `Test/.../UnitTests/Helpers/TestHandler.cs` | Capturing `HttpMessageHandler` for pipeline tests |

## Conventions

- **TestCommon for non-test fixtures** (`ObjectMother`, `FakeCache`, `FakeStorage`) — available to all test projects
- **Test-specific fixtures** in the test project itself (`AuthHandlerTestFixture` family, `TestHandler`) — scoped to unit tests
- **Abstract fixture + concrete implementations** for polymorphic tests — each integration provides its own concrete fixture with integration-specific setup
- **`AuthHandlerTestFixture` implements `IXunitSerializable`** for `[ClassData]` serialization
- **`AuthHandlerFixtureProvider` is a `TheoryData<AuthHandlerTestFixture>`** that registers all concrete fixtures
- **Shared setup methods** (like `CreateDefaultContext()`, `CreateCacheMock()`) are `protected static` on the abstract fixture
- **`ObjectMother` methods** provide sensible defaults with optional parameters for test-specific overrides

## Anti-Patterns

- ❌ **Duplicating container lifecycle** across test classes — use `IAsyncLifetime` collection fixtures for TestContainers
- ❌ **Fixture-per-test antipattern** — if setup is shared across many tests, extract to `IClassFixture<T>` or constructor
- ❌ **Placing infrastructure-dependent fixtures** (e.g., container-dependent) in `TestCommon` — keep in the integration test project
- ❌ **One fixture doing too much** — `AuthHandlerTestFixture` handles auth; don't add cache, logging, or unrelated concerns to it
- ❌ **Mutable shared state** across tests — fixtures should be stateless or reset per test via constructor

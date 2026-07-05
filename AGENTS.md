# AGENTS.md — AI Coding Agent Reference

## Repository Architecture

**Hexagonal (Ports & Adapters) architecture with DDD-inspired layering.**

### Layer Structure

```
Src/Orders.Integrations.Hub/
  Core/
    Domain/         — Entities, ValueObjects, Enums (zero dependencies)
    Application/    — Commands, DTOs, Ports (interfaces only)
    Adapters/       — In/ (HTTP, Messaging), Out/ (HttpClients, Cache)
    Infrastructure/ — Exceptions, Extensions, Integration, Messaging, Middlewares, Serialization
  Integrations/
    Common/         — Shared contracts, middleware, serialization, validators
    IFood/          — Self-contained module with Domain/Application/Adapters/Infrastructure
    Rappi/          — Same pattern
    Food99/         — Same pattern
```

### Key Technical Decisions

- **MassTransit** for messaging — InMemory for dev, RabbitMQ for prod (see `CoreDependencyInjection.cs:AddMessageBroker`)
- **Hybrid cache** — L1 MemoryCache + L2 Redis via `ICacheService`, switchable via `CACHE_MODE` env var
- **AWS S3** for dispute evidence storage via `IObjectStorageClient`
- **Minimal APIs** with auto-discovery: endpoints implement `IEndpoint` interface, registered via `AddEndpoints()` + `MapEndpoints()`
- **Keyed DI services** resolved via `IIntegrationRouter` — each integration registers its use cases with its `IntegrationKey`, router resolves at runtime
- **Refit** for typed HTTP clients per integration
- **NetArchTest** for architecture enforcement in arch tests

### Integration Module Pattern

Each integration (IFood, Rappi, Food99) follows the same internal structure:
```
IFood/
  Domain/       — Contracts, Entity, ValueObjects
  Application/  — Ports (In/Out), Clients, Handlers, ValueObjects
  Adapters/     — Endpoints (implement IEndpoint)
  Infrastructure/ — Signature strategies, serializers
```

Integrations are **fully isolated** — they must not depend on each other. They depend on `Integrations/Common` for shared contracts and on `Core/Domain` for value objects like `IntegrationKey`.

### Testing Philosophy

- **Value maintainability over raw coverage** — tests should be easy to read and refactor
- **One behavior per test** — each test verifies a single expected outcome
- **AAA (Arrange, Act, Assert)** as explicit comment blocks or clear structural separation
- **Avoid over-mocking** — mock only external boundaries (HTTP, cache, SNS, S3). Prefer real implementations when practical
- **Use builders/object mothers** (`TestCommon/Fakers/`) over inline setup
- **No sleeps, no timing issues** — use `ITestHarness`, `TaskCompletionSource`, or virtual time
- **Tests are independent and parallelizable** — shared state only via collection fixtures

### Naming Conventions

| Test Type | Pattern | Example |
|---|---|---|
| Unit test | `{Method}_Should_{ExpectedBehavior}_When_{Condition}` | `SendAsync_ShouldSkipAuth_WhenAuthorizationHeaderAlreadySet` |
| MassTransit handler | `Consume_Should_{Action}_When_{Condition}` | `Consume_Should_Publish_To_SNS_With_TopicArn_From_Command` |
| Polymorphic fixtures | `Theory` + `ClassData` | `AuthMessageHandlerTests` uses `AuthHandlerFixtureProvider` + `AuthHandlerTestFixture` |

Test files mirror source namespace structure:
- Source: `Src/.../Core/Infrastructure/Serialization/CoreJsonSerializer.cs`
- Test: `Test/.../UnitTests/Serialization/CoreJsonSerializerTests.cs`

### AAA Rules

1. **Arrange** — set up test data, mocks, and system under test (SUT). Use Fakers from `TestCommon`, set up mocks with `Substitute.For<>`
2. **Act** — invoke the single behavior under test. One action only
3. **Assert** — verify the single expected outcome. Use standard `Assert.*`, `Received(1)`, or custom assertion helpers

One `Act` per test. No chained behaviors.

### How Agents Should Work

1. **Read source code first** — understand the existing pattern before writing new code
2. **Check for existing builders/fixtures** in `Test/Orders.Integrations.Hub.TestCommon/` before creating new ones
3. **Follow existing patterns** — look at similar tests for structure, naming, and assertions
4. **Run tests** — both unit and integration:
   ```
   dotnet test Test/Orders.Integrations.Hub.UnitTests
   dotnet test Test/Orders.Integrations.Hub.IntegrationTests
   ```
5. **Prefer adding to existing test files** over creating new ones, unless a new integration or feature area warrants a new file
6. **For TestContainers** — use `IAsyncLifetime` collection fixtures (see existing integration tests for pattern)

### How New Tests Should Be Added

1. Check if `TestCommon/Fakers/` or `TestCommon/Fixtures/` has a builder/faker that fits. Use `ObjectMother` for common pre-built objects
2. **New integration (e.g., KEETA):** create tests following the existing integration pattern:
   - Serializer tests (mirror `RappiJsonSerializerTests`)
   - Auth handler fixture + tests (mirror `AuthHandlerFixtureProvider` + `AuthMessageHandlerTests`)
   - Endpoint tests using `WebApplicationFactory` + `TestContainers.Redis`
   - Mapping/extension tests that deserialize real payloads and assert mapped fields
3. **Serializers:** create a test class following `Food99JsonSerializerTests` / `RappiJsonSerializerTests` pattern — test camelCase, snake_case enums, round-trip
4. **Webhook endpoints:** use `WebApplicationFactory` + `TestContainers.Redis` approach (see integration test patterns)
5. **Mapping extensions:** create a test that deserializes a real payload fixture and asserts all mapped fields on the domain object
6. **Arch tests auto-discover** new integrations via `DiscoverIntegrationNamespaces()` — no changes needed in arch tests

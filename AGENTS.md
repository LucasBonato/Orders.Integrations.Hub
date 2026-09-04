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
- **Hybrid cache** — L1 MemoryCache + L2 Redis via `ICacheService`, switchable via `Cache:Provider` config (default in `appsettings.json`, overridable via the dotenv chain)
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

## Integration Test Architecture

`Test/Orders.Integrations.Hub.IntegrationTests/` mirrors the source layout:

```
IntegrationTests/
  Contracts/            — IIntegrationContract per integration + IntegrationContractRegistry
    Signing/            — IWebhookSigner per integration (IFoodSigner, RappiSigner, Food99Signer)
    {IFood,Rappi,Food99}/ — contract descriptors (integration key, signer, payload catalog)
  Entrypoints/
    Http/               — generic Orders-Hub endpoints tested as theories over all contracts
    Messaging/          — REAL-RabbitMQ round trips: webhook → bus → consumer → IOrderClient
    Webhooks/           — shared WebhookSignatureTests (missing/invalid/valid theory) + per-integration dispatch
  Infrastructure/
    Aws/                — LocalStackAwsClients (S3/SNS/SQS) + infra-backed tests (S3 dispute evidence)
    Host/               — AppFactory (WebApplicationFactory), WireMockServers, HostConfiguration
    Fixtures/           — TestInfrastructure collection fixture (+ co-located IntegrationTestCollection),
                          container fixtures and module initializer (ContainerHostModule)
  Payloads/
    Templates/{IFood,Rappi,Food99}/ — loose JSON payload templates (copied to output via csproj Content)
    PayloadLoader.cs    — raw body loading + Deserialize<T> (signature is over the raw body)
    PayloadBuilder.cs   — JsonNode dotted-path Set/Remove/Get for payload mutation
  Requests/
    WebhookClient.cs    — PostSignedAsync/PostWebhookAsync helpers
  Builders/
    TestDataFactory.cs  — test data builders
```

### Parallel collections — enabled (no process env races)

Host configuration loads static values from `appsettings.IntegrationTest.json` and supplies
runtime-discovered WireMock/container values through `UseSetting` (bridged into application
configuration by `HostConfiguration`); **no test host writes process env vars**. `AppFactory`
always uses the `Test` environment, which also prevents `.env` loading in `Program.cs`. So
`xunit.runner.json` can keep `"parallelizeTestCollections": true`.

The real-infra collection (`IntegrationTestCollection`) still runs its tests serially within the
collection (single shared fixture), while all in-memory collections run in parallel with it.

### Configuration layering (prod)

Config is read through two channels, split by *who varies per test host*:

- **`IConfiguration`** — all application settings, including integration credentials and
  runtime test overrides. Sources, lowest to highest: `appsettings.json` →
  `appsettings.{env}.json` → dotenv chain → real env vars → per-host test `UseSetting`.
  `ConfigurationExtensions` (`Required`/`GetOr`/`IsTrue`) read this channel.

The **dotenv chain** (`Program.cs` `LoadEnvFiles()`, skipped when
`ASPNETCORE_ENVIRONMENT=Test`) loads `.env` (committed defaults) → `.env.{environment}` →
`.env.local` (both gitignored) into process env; later files win; variables already in the
real environment always win. Loaded variables reach `IConfiguration` through the
environment-variables provider.

### Two host modes

- **In-memory host** — `AppFactory.Create()`. Cache=Memory, broker=InMemory, no
  containers. Used for webhook dispatch, signature theory, Core endpoint theories, mapping tests.
- **Real-infra host** — `AppFactory.Create(TestInfrastructure.Environment)` inside
  `[Collection(IntegrationTestCollection.Name)]` (fixture injected via ctor). Cache=Hybrid (real
  Redis), broker=RabbitMq (real RabbitMQ), and S3/SNS on real LocalStack. LocalStack resources
  are created directly with the AWS SDK. Requires Docker or Podman on PATH.

  **Container engine**: TestContainers auto-discovers the engine. On Windows with **Podman** the
  default pipe is already bound to `\\.\pipe\docker_engine`, so **do NOT** pre-set `DOCKER_HOST`.
  For a **rootful WSL podman machine** (no `UserModeNetworking`), published ports live inside the
  WSL VM and are not on `localhost`. A `[ModuleInitializer]` (`ContainerHostModule`) resolves the
  VM IP from `wsl -d podman-machine-default -- ip -4 addr show eth0` and sets
  `TestcontainersSettings.DockerHostOverride`, so **no manual `TESTCONTAINERS_HOST_OVERRIDE` is
  required** (in Rider, CI, or the CLI). The env var is still honored when set (e.g. a different
  machine name). The test reaper is disabled in `TestInfrastructure` (fixtures dispose containers
  via `IAsyncLifetime`). When setting env vars on the Windows cmd line always use the quoted form
  `set "VAR=value" && ...` — the unquoted form `set VAR=value && ...` appends a trailing space to
  the value, which breaks Testcontainers' hostname parsing (`UriFormatException: The hostname could
  not be parsed` in localstack/RabbitMq/redis/Redis start).
  Requires **Docker or Podman** on PATH. Consumers publish to SNS / call `IOrderClient` for real.

### External boundaries

WireMock owns the Orders, IFood, Rappi, and Food99 HTTP boundaries. The current production
`InternalClient` is an in-code settings adapter, so tests replace only that adapter with
`TestIntegrationClient` while retaining `InternalCacheClient`. No external HTTP boundary is
mocked with NSubstitute.

### Deterministic async assertions (no sleeps)

- Consumer round trips: wait for a WireMock request with `WaitForCreateOrderAsync` or the
  corresponding request helper, then assert its count/body.
- SNS delivery: subscribe a temp SQS queue to the topic, then long-poll `ReceiveMessageAsync(WaitTimeSeconds: 20)`.
- Messaging tests use the real RabbitMQ transport for consumer round trips; there is no separate
  in-memory harness layer for these integration tests.

### Payloads

Realistic loose JSON per integration in `Payloads/Templates/{integration}/{name}.json`; signature is computed
over the **raw body**, so payload files are never re-serialized by tests (`PayloadLoader.LoadRaw`).
`PayloadLoader.Deserialize<T>` exists for mapping assertions against app serializers.

### Testing Philosophy

- **Value maintainability over raw coverage** — tests should be easy to read and refactor
- **One behavior per test** — each test verifies a single expected outcome
- **AAA (Arrange, Act, Assert)** as explicit comment blocks or clear structural separation
- **Avoid over-mocking** — mock only external boundaries (HTTP, cache, SNS, S3). Prefer real implementations when practical
- **Use builders/object mothers** (`TestCommon/Fakers/`) over inline setup
- **No sleeps, no timing issues** — use `TaskCompletionSource` signals, SQS long-poll, or virtual time
- **Collections are parallel** (see above) — shared real infra only via the `IntegrationTestCollection` fixture

### Naming Conventions

| Test Type | Pattern | Example |
|---|---|---|
| Unit test | `{Method}_Should_{ExpectedBehavior}_When_{Condition}` | `SendAsync_ShouldSkipAuth_WhenAuthorizationHeaderAlreadySet` |
| MassTransit consumer | `Publish_Should_{Action}` | `Publish_ShouldPostOrderToOrdersService` |
| Webhook dispatch | `{Event}_Should_{Action}` | `OrderNew_Should_Call_CreateOrder` |
| Core theory | `{Fact}_Returns_{Expected}_ForEveryIntegration` | `Get_ShouldReturnCancellationReasons_ForEveryIntegration` |
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
6. **For TestContainers** — use `IAsyncLifetime` collection fixtures (see existing integration tests for pattern); requires Docker or Podman on PATH

### How New Tests Should Be Added

1. Check if `TestCommon/Fakers/` or `TestCommon/Fixtures/` has a builder/faker that fits. Use `ObjectMother` for common pre-built objects
2. **New integration (e.g., KEETA):** create tests following the existing integration pattern:
   - Serializer tests (mirror `RappiJsonSerializerTests`)
   - Auth handler fixture + tests (mirror `AuthHandlerFixtureProvider` + `AuthMessageHandlerTests`)
   - `IIntegrationContract` + signer (mirror `Food99Contract` + `Food99Signer`) so the shared
     signature theory, Core endpoint theories, and `IntegrationContractRegistry` pick it up automatically
   - Webhook dispatch tests (mirror `Food99WebhookDispatchTests`, using `WebhookClient` + a payload file)
   - Mapping/extension tests that deserialize real payloads and assert mapped fields
3. **Serializers:** create a test class following `Food99JsonSerializerTests` / `RappiJsonSerializerTests` pattern — test camelCase, snake_case enums, round-trip
4. **Webhook endpoints:** use `AppFactory.Create()` in-memory host; post the raw payload
   through `WebhookClient.PostSignedAsync`; don't serialize the payload again (signature is over raw body)
5. **Consumer/infra round trips:** use the real-infra host inside `[Collection(IntegrationTestCollection.Name)]`
   with a `TaskCompletionSource` wired into a factory substitute; never re-add sleeps
6. **Mapping extensions:** create a test that deserializes a real payload fixture and asserts all mapped fields on the domain object
7. **Arch tests auto-discover** new integrations via `DiscoverIntegrationNamespaces()` — no changes needed in arch tests

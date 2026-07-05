# hexagonal-testing

**When:** Testing hexagonal architecture boundaries — verifying port-adapter isolation, layer dependencies, and integration module independence.

## Patterns

- **Port mocking:** Mock outbound ports (e.g., `IOrderClient`, `ICacheService`, `IAmazonSNS`) when testing inbound adapters
- **Adapter isolation:** Test each adapter against its port interface, not against the concrete implementation of other adapters
- **Architecture enforcement via NetArchTest:** `CoreArchitectureTests.cs` and `IntegrationsArchitectureTests.cs` verify layer dependencies at compile time
- **IntegrationRouter for keyed DI:** Test that integrations register their use cases with `IntegrationKey` and resolve at runtime (see `IntegrationRouterTests`)

## Examples

| Test Class | File Path | What It Tests |
|---|---|---|
| `CoreArchitectureTests` | `Test/.../ArchTests/CoreArchitectureTests.cs` | Domain independence, Application-only-on-Domain, adapter placement |
| `IntegrationsArchitectureTests` | `Test/.../ArchTests/IntegrationsArchitectureTests.cs` | Integration isolation, common contracts, auto-discovery |
| `IntegrationRouterTests` | `Test/.../UnitTests/Integration/IntegrationRouterTests.cs` | keyed DI resolution, scoped lifetime, missing key |
| `MassTransitCommandDispatcher` (via `CommandDispatcherTests`) | `Test/.../IntegrationTests/CommandHandlers/CommandDispatcherTests.cs` | adapter `ICommandDispatcher` → bus publish |

## Layer Dependency Rules

| Layer | May Depend On | Must Not Depend On |
|---|---|---|
| `Domain` | Nothing | `Application`, `Infrastructure`, `Adapters`, Integrations |
| `Application` | `Domain` | `Infrastructure`, `Adapters`, Integrations |
| `Infrastructure` | `Application`, `Domain` | `Adapters` |
| `Adapters.In` | `Application` (ports) | `Adapters.Out` |
| `Adapters.Out` | `Application` (ports), `Infrastructure` | `Adapters.In` |
| Integration modules | Its own layers + `Common` + `Core.Domain` | Other integrations, `Core.Adapters`, `Core.Infrastructure` |

## Conventions

- **Architecture tests auto-discover** integration namespaces via `DiscoverIntegrationNamespaces()` — adding a folder is enough
- **Port interfaces** use the `I{Name}UseCase` naming convention for use cases, `I{Name}Client` for outbound clients
- **Inbound adapters** (command handlers, webhook endpoints) implement `IConsumer<T>` or `IEndpoint`
- **Outbound adapters** (HTTP clients, cache implementations) are tested against mocked ports
- **Test the adapter through the port interface** — never instantiate the adapter without using its interface
- **Arch tests are `[Fact]`** for static rules, `[Theory] [MemberData(nameof(IntegrationNamespaces))]` for per-integration rules

## Anti-Patterns

- ❌ **Circular dependencies** — arch tests will catch these; fix at design time, not test time
- ❌ **Integration modules depending on each other** — each integration is fully isolated; use `Common` for shared logic
- ❌ **Application layer referencing infrastructure** — application defines ports, infrastructure implements them; never the reverse
- ❌ **Testing adapters against real implementations of other adapters** — mock the ports of non-SUT adapters
- ❌ **Skipping arch tests on refactors** — arch tests are safety nets against layer erosion; fix violations immediately

# Testing Strategy

## Overview

This repository uses a **risk-based testing strategy** adapted for a hexagonal (ports & adapters) event-driven integration platform. The goal is not blanket coverage but targeted confidence where it matters most: serialization, mapping, and message handling.

## Test Pyramid (Adapted)

```
     ╱╲          E2E (few — manual smoke or staging only)
    ╱  ╲
   ╱────╲        Integration (MassTransit harness, TestContainers)
  ╱      ╲
 ╱────────╲      Unit (serializers, validators, routers, handlers in isolation)
╱──────────╲
```

The pyramid is **flattened** — we write more unit tests and focused integration tests, but very few (if any) full end-to-end tests. The event-driven architecture means most bugs surface at the serialization, routing, or mapping boundaries, not at the UI level.

## What Each Level Covers

| Layer | Test Level | What's Tested |
|---|---|---|
| **Domain** (Core/Domain) | Unit | Entities, ValueObjects, Enums — pure logic, zero dependencies |
| **Application** (Core/Application) | Unit + Integration | Commands, DTOs, Port interfaces, UseCase orchestration |
| **Adapters.In** (HTTP, Messaging) | Integration | Endpoint filters, webhook signature validation, command handlers |
| **Adapters.Out** (Cache, HTTP, S3) | Unit + Integration | Cache behavior, HTTP client pipelines, object storage operations |
| **Infrastructure** | Unit | Serializers (`ICustomJsonSerializer`), routers (`IntegrationRouter`), middleware, converters |
| **Integrations** (IFood, Rappi, Food99) | Unit + Integration | Per-integration serializers, signature strategies, auth handlers, pipeline handlers |

## Risk-Based Approach

Highest-risk areas get disproportionate test investment:

- **Serialization / Deserialization** — every integration has its own JSON format (camelCase, snake_case, enum casing differences). A serializer mismatch causes silent data corruption, not a crash. Each integration serializer gets a dedicated test class.
- **Mapping Extensions** — converting integration-specific DTOs to domain objects. Tested with real payloads deserialized from files.
- **Signature Validation** — webhook security. Every validator is tested for valid, tampered, missing, and expired signatures.
- **Message Routing** — `IIntegrationRouter` resolution by keyed DI. A wrong route means silent message loss.
- **Command Handlers** — MassTransit consumers. Tested with `ITestHarness` to verify consumption, fault publishing, and mock interactions.

## Adding a New Integration

To add a new integration (e.g., "Keeta") with minimal test code:

1. **Serializer tests** — follow `RappiJsonSerializerTests` pattern (camelCase, snake_case, enums, round-trip)
2. **Auth handler tests** — use `AuthHandlerTestFixture` + `AuthHandlerFixtureProvider` polymorphic pattern
3. **Pipeline tests** — chain `IntegrationContextHandler` + auth handler + `TestHandler` to verify end-to-end request flow
4. **Webhook endpoint tests** — use `WebApplicationFactory` with `TestContainers.Redis`
5. **Mapping extension tests** — deserialize a real payload fixture and assert mapped fields on the domain object

Arch tests auto-discover new integrations via `DiscoverIntegrationNamespaces()` — no modifications needed.

## Reusable Infrastructure

- `Test/Orders.Integrations.Hub.TestCommon/` contains shared builders, fakes, and fixtures
- `FakeCache` and `FakeStorage` replace Redis/S3 for unit tests
- `ObjectMother` provides static factory methods for common domain objects
- `AuthHandlerTestFixture` + `ClassData` enables testing auth handlers across all integrations with shared test methods
- `MassTransitTestHarnessExtensions.AddDefaultTestHarness<T>()` reduces boilerplate for consumer tests

## Coverage Targets

No hard numbers — but these guidelines apply:

- **Serializers**: every serializer method (Serialize, Deserialize, enums, null handling, round-trip) must be tested
- **Validators**: every code path (valid, invalid, boundary) must be tested
- **Infrastructure**: the `IntegrationRouter` Resolve/CanResolve/throw paths must be tested
- **Command handlers**: consume success + fault paths must be tested
- **Cache**: Get/Set/expire/overwrite for each cache mode must be tested
- **Middleware**: each exception type mapping must be tested

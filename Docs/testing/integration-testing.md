## Overview

Integration tests validate the real wiring between the HTTP pipeline, message bus, cache, storage, and external HTTP boundaries.

## Test Hosts

| Host | Use |
|---|---|
| `AppFactory.Create()` | In-memory broker/cache, WireMock HTTP boundaries, fast endpoint and webhook tests |
| `AppFactory.Create(environment)` | RabbitMQ, Redis, and LocalStack integration tests |

Both hosts run the real application entry point. The test host always uses the `Test` environment and committed `appsettings.IntegrationTest.json`; runtime container and WireMock values are injected per host.

## External Boundaries

- WireMock owns Orders, IFood, Rappi, and Food99 HTTP services.
- `TestIntegrationClient` replaces the in-code `InternalClient` settings adapter while retaining `InternalCacheClient`.
- RabbitMQ tests publish through `IPublishEndpoint` and observe the resulting HTTP call.
- S3, SNS, and SQS tests use LocalStack through the AWS SDK.

## Containers

`TestInfrastructure` is a shared, serial collection fixture for Redis, RabbitMQ, and LocalStack. The fixture resets state before each test instance. Docker or Podman is required for the real-infrastructure tests.

## Deterministic Coordination

- Use WireMock wait helpers for HTTP effects.
- Use SQS long-polling for SNS delivery.
- Use `TestContext.Current.CancellationToken`.
- Do not use `Task.Delay` or process environment mutation for test coordination.

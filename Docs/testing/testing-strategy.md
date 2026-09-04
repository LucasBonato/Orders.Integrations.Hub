## Overview

This repository uses a risk-based testing strategy for a hexagonal, event-driven integration platform. The goal is targeted confidence at serialization, mapping, routing, transport, and external-service boundaries.

## Test Pyramid

```
     /\        Manual or staging smoke tests
    /  \
   /----\      Integration: HTTP, RabbitMQ, Redis, LocalStack
  /      \
 /--------\    Unit: domain, serializers, validators, routers, handlers
```

## What Each Level Covers

| Area | Test level | Coverage |
|---|---|---|
| Core domain | Unit | Entities, value objects, enums, and pure rules |
| Application | Unit + integration | Commands, DTOs, ports, and use-case orchestration |
| Inbound adapters | Integration | HTTP endpoints, webhook signatures, and MassTransit consumers |
| Outbound adapters | Unit + integration | HTTP clients, cache, S3, SNS, and serialization |
| Integration modules | Unit + integration | Mapping, auth, signatures, pipelines, and dispatch |

## Integration Host Modes

- `AppFactory.Create()` uses in-memory MassTransit and memory cache. WireMock owns the Orders and integration HTTP boundaries.
- `AppFactory.Create(testInfrastructure.Environment)` uses real RabbitMQ, Redis, and LocalStack containers. These tests are in `IntegrationTestCollection` and run serially because they share the fixture.
- Runtime container and WireMock values are supplied through `UseSetting` and a host configuration provider. Tests do not write process environment variables.
- Consumer round trips wait for observable WireMock or SQS results. Tests do not use sleeps.

## Risk-Based Priorities

- Test every integration serializer and mapping extension with realistic payloads.
- Test webhook signatures for missing, invalid, and valid signatures.
- Test keyed integration routing and unsupported integrations.
- Test consumer round trips through real RabbitMQ when transport wiring matters.
- Test S3/SNS behavior against LocalStack when an in-memory fake cannot verify the behavior.

## Adding an Integration

1. Add serializer, auth, pipeline, mapping, and webhook tests using the existing integration patterns.
2. Add an `IIntegrationContract` and signer so shared theories discover the integration automatically.
3. Add realistic raw payload templates. Signatures must be calculated over the raw body.
4. Run the unit, architecture, and integration test projects.

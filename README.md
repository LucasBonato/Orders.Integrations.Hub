# Integrations Hub for Orders

**`Orders.Integrations.Hub`** is a modular and extensible integration hub built with **.NET 10**, designed to receive and 
standardize external orders from platforms like iFood, Rappi, **99Food**, and more. It forwards these orders to an internal 
system using a clean and decoupled architecture.

## 🔗 Summary

- [🔭 Overview](#-overview)
- [🧩 Architecture Diagram](#-architecture-diagram)
- [📁 Project Structure](#-project-structure)
- [🧱 Integration Architecture](#-integration-architecture)
    - [➕ Adding a New Integration](#-adding-a-new-integration)
    - [🔑 Integration Keys & Routing](#-integration-keys--routing)
    - [🧩 Use Cases Overview](#-use-cases-overview)
    - [🏪 Multi-Tenant Support](#-multi-tenant-support)
    - [🏗 Centralized Credentials Mode](#-centralized-credentials-mode)
    - [🧾 Per-Integration JSON Serialization](#-per-integration-json-serialization)
- [🔄 Communication Flow](#-communication-flow)
- [🧪 Testing](#-testing)
- [🧪 Observability & Monitoring](#-observability--monitoring)
- [🧰 Local Development](#-local-development)
    - [🐳 With Docker Compose](#-with-docker-compose)
    - [☁️ Infrastructure](#-infrastructure)
    - [🔧 Configuration](#-configuration)
- [🔧 Tech Stack](#-tech-stack)
- [🧠 Future Improvements](#-future-improvements)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

---

## 🔭 Overview

This project serves as a central entry point for receiving and processing external orders. It provides:

- 🌐 **Minimal APIs**
- 📃 **Scalar** (OpenAPI alternative) for documentation
- 🔄 **Command-driven** internal communication using **MassTransit** message bus
- 🔌 **Pluggable integrations** (e.g., Rappi, iFood, **99Food**)
- 🔑 **Type-safe integration routing** with `IntegrationKey` value objects
- 🏪 **Multi-tenant support** (per-merchant credentials via internal API + caching)
- 🧾 **Custom JSON serialization per integration** (no `JsonPropertyName` attributes)
- 📦 **Flexible Caching** — in-memory (L1), Redis (L2), or hybrid L1/L2 modes
- 📊 **Observability** with OpenTelemetry
- ☁️ **Infrastructure as Code** with Terraform and LocalStack
- 🧪 **Comprehensive Test Suite**: Unit, Architecture, and Integration tests with MassTransit test harness

---

## 🧩 Architecture Diagram

> 📌 You can find the diagram in: [`Docs/Architecture.png`](./Docs/Architecture-Light.png)

![Architecture Diagram](Docs/Architecture-Dark.png)

Key elements:
- Inbound integrations via Webhooks or Pulling
- Each integration (**iFood**, **Rappi**, **99Food**, etc.) has isolated flows
- Events are created and consumed asynchronously
- All inbound calls use the `UseCases.In` contracts to reach internal services (e.g., Orders API)
- All outbound calls use the `UseCases.Out` contracts to reach external services (e.g, Integration API)

---

## 📁 Project Structure

```text
Orders.Integrations.Hub/
├── Src/
│   └── Orders.Integrations.Hub/
│       ├── Core/
│       │   ├── Adapter/        # HTTP endpoints (Minimal APIs)
│       │   ├── Application/    # Middlewares, Extensions, Use Cases
│       │   ├── Domain/         # Contracts, Entities, Value Objects
│       │   └── Infrastructure/ # Implementations, Extensions, Handlers 
│       │
│       └── Integrations/       # Modular integrations: Ifood, Rappi, 99Food, etc.
│           ├── Common/         # Shared utilities across integrations
│           ├── IFood/          # IFood integration
│           ├── Rappi/          # Rappi integration
│           ├── Food99/         # 99Food integration
│           └── .../            # More integrations
│
├── Test/
│   ├── Directory.Build.props                       # Build properties for tests
│   ├── Directory.Packages.props                    # Packages used on the tests 
│   ├── Orders.Integrations.Hub.UnitTests/          # Unit tests
│   ├── Orders.Integrations.Hub.IntegrationTests/   # Integrations tests
│   ├── Orders.Integrations.Hub.TestCommon/         # Shared utilities across tests
│   └── Orders.Integrations.Hub.ArchTests/          # Architecture tests
│
├── Infra/
│   └── terraform/              # Infrastructure code using Terraform
│
├── .env.example                # Example of the necessary environment variables
└── docker-compose.yml          # Local dev environment
```

---

## 🧱 Integration Architecture

Each integration is encapsulated in its own folder and follows its own mini-architecture. This makes it easy to add or 
evolve integrations independently.

### ➕ Adding a New Integration
1. **Create the integration folder** under `Integrations/IntegrationName`.
2. **Define the Integration Key** with validation:
```csharp
   // Integrations/IntegrationName/IntegrationNameIntegrationKey.cs
   [IntegrationKeyDefinition]
   public static class IntegrationNameIntegrationKey 
   {
       public const string Value = "INTEGRATION_NAME";
       
       public static readonly IntegrationKey INTEGRATION_NAME = IntegrationKey.From(Value);
       
       static IntegrationNameIntegrationKey() {
           IntegrationKeyValidator.ValidateRawValue(Value);
       }
   }
```
2. **Implement the architecture** that you prefer (adapters, use cases, clients).
3. **Register dependencies** in `Integrations/IntegrationName/IntegrationNameDependencyInjection.cs`:
```csharp
    public static IServiceCollection AddIntegrationNameIntegration(this IServiceCollection services) {
        
        services.AddKeyedScoped<IOrderChangeStatusUseCase, IntegrationNameOrderChangeStatusUseCase>(IntegrationNameIntegrationKey.Value);
        
        // ...
        
        return services;
    }
```
4. **(Optional)** Provide a custom JSON serializer (see below).
5. **(Optional/Recommended)** Implement the Use Cases for your integration

---

### 🔑 Integration Keys & Routing

The hub uses a type-safe `IntegrationKey` value object instead of enums to achieve extensibility and avoid coupling the
Core domain to specific integrations.

#### **IntegrationKey Value Object**
```csharp
public sealed record IntegrationKey {
    public string Value { get; }
    
    private IntegrationKey(string value) => Value = value;
    
    public static IntegrationKey From(string value) => new(value.Trim().ToUpperInvariant());
}
```

#### **Integration Router**

The `IIntegrationRouter` dynamically resolves use cases based on the integration key at runtime:

**Usage in Controllers:**
```csharp
public static async Task<Ok<List>> GetIntegrationCancellationReason(
    [FromQuery] string? externalOrderId,
    [FromQuery] IntegrationKey integration, // Automatically parsed from query string
    [FromServices] IIntegrationRouter router
) {
    var useCase = router.Resolve<IOrderChangeStatusUseCase>(integration);
    return TypedResults.Ok(await useCase.ExecuteAsync(externalOrderId));
}
```

**HTTP Request Example:**
```http
GET /api/orders/cancellation-reasons?externalOrderId=123&integration=ifood
```
- The `integration` parameter (`"ifood"`) is automatically normalized to `"IFOOD"`
- The router resolves the iFood-specific use case implementation
- Type-safe at compile time, flexible at runtime

#### **Benefits of IntegrationKey**

✅ **Extensible**: Add new integrations without modifying Core domain  
✅ **Type-safe**: Strongly typed with compile-time validation  
✅ **Normalized**: Automatic uppercase normalization prevents DI mismatches  
✅ **Validated**: Runtime validation catches configuration errors at startup

---

### 🧩 Use Cases Overview

⚠️ **Note:** All use cases are optional.  
Implement only the ones supported or required by the partner integration.

| Use Case                            | Direction | When to use                                                                                                |
|-------------------------------------|-----------|------------------------------------------------------------------------------------------------------------|
| `IOrderCreateUseCase`               | IN        | Triggered when a new order is received from the integration and must be sent to the core.                  |
| `IOrderDisputeUseCase`              | IN        | Handles disputes (started/finished) from the integration and forwards them to the core.                    |
| `IOrderUpdateUseCase`               | IN        | Used when an order changes its status in the integration and the core must be updated.                     |
| `IOrderChangeProductStatusUseCase`  | OUT       | Used when the core needs to enable/disable products in the partner system (e.g. stock, catalog).           |
| `IOrderChangeStatusUseCase`         | OUT       | Sends status changes made by the merchant in the core to the integration (e.g. confirm, preparing, cancel).|
| `IOrderDisputeRespondUseCase`       | OUT       | Sends the merchant’s response to a dispute back to the integration.                                        |
| `IOrderGetCancellationReasonUseCase`| OUT       | Retrieves cancellation reasons supported by the integration (via API or enums).                            |

📌 **Summary**
- **IN** → Events coming **from the integration into the Hub** (orders, disputes, updates).
- **OUT** → Events going **from the Hub to the integration** (status changes, catalog updates, disputes, cancellations).

### 🏪 Multi-Tenant Support

Some platforms (like **iFood**, **Rappi**, **99Food**) require **per-merchant credentials** (ClientId, ClientSecret, 
StoreId/MerchantId, etc.).  
This hub supports **multi-tenancy** through a scoped `IntegrationContext`:

- Each HTTP request creates its own **scoped `IntegrationContext`**.
- The context fetches integration metadata (client credentials, merchant/store identifiers, base URLs, etc.) from an **internal API**.
- A **cache decorator** prevents fetching on every request.

**Example (iFood):**
```csharp
integrationContext.Integration = (await internalClient.GetIntegrationByExternalId(request.MerchantId)).ResolveIFood();

integrationContext.MerchantId = request.MerchantId;

string secret = integrationContext.Integration.ClientSecret;
```

This lets the hub adapt dynamically per merchant instead of relying on static environment variables.

---

### 🏗 Centralized Credentials Mode

Not all partners require per-merchant credentials, or you have a centralizer. For those cases, you can **remove the 
`IntegrationContext` dependency** for that integration and bind credentials via environment variables.

**How to switch an integration to centralized mode:**

1. **Remove context usage**: Stop injecting/using `IntegrationContext` in that integration’s handlers/clients.
2. **Bind env vars**: Configure static credentials in `.env` and map them to typed options:
   ```ini
   INTEGRATIONS__PARTNER__CLIENT__ID=your-client-id
   INTEGRATIONS__PARTNER__CLIENT__SECRET=your-client-secret
   INTEGRATIONS__PARTNER__ENDPOINT__BASE_URL=https://api.partner.com
   ```
3. **Register options & client**: Inject the options directly into the integration services (no lookup/caching required).
4. **Validate at startup**: Fail fast if required keys are missing when centralized mode is chosen.

> ✅ Use **multi-tenant** when each merchant has its own credentials.  
> ✅ Use **centralized** when one credential set is shared across all merchants.

---

### 🧾 Per-Integration JSON Serialization

Each integration owns a **custom JSON serializer** (for request/response models), injected via DI using **keyed services**:

Implement the interface `ICustomJsonSerializer` for the integration that you want with all configurations

```csharp
public class RappiJsonSerializer : ICustomJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new() {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper) }
    };

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    public T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
}
```

Then in the DI you just need to add the service now with the integration key

```csharp
services.AddKeyedSingleton<ICustomJsonSerializer, RappiJsonSerializer>(RappiIntegrationKey.Value);
```

Now just use it in your integration

```csharp
public IResult Handle(
    [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    /* other deps */
) {
    // Use the serializer to parse/format payloads for this integration
}
```

This decouples payload shapes per partner and avoids leaking partner-specific casing/naming into shared domain models.

---

## 🔄 Communication Flow

### High-Level Flow

1. **Inbound**: External platforms (**iFood**, **Rappi**, **99Food**, etc.) send data via webhooks or pulling
2. **Adapter**: Requests hit the corresponding endpoint (e.g., `IfoodController`)
3. **Translation**: Adapter translates the request into internal format
4. **Command Creation**: The use case creates a **Command** (e.g., `CreateOrderCommand`)
5. **Dispatch**: Command is dispatched via `ICommandDispatcher` to the message bus
6. **Message Queue**: Command is queued (in-memory for dev, RabbitMQ for production)
7. **Handler**: `CommandHandler` consumes and processes the command asynchronously
8. **Outbound**: Use cases send data via `UseCases.Out` contracts to internal systems

### Command Flow Diagram

```mermaid
graph TB
    A["External Platform<br/>(iFood, Rappi, 99Food)"] -->|"Webhook / Polling"| B["HTTP Endpoint<br/>(Controller)"]
    
    B -->|"Translate"| C["Adapter<br/>(Request → Internal)"]
    
    C -->|"Create"| D["Command<br/>(CreateOrderCommand)"]
    
    D -->|"Dispatch"| E["ICommandDispatcher<br/>(Infrastructure Layer)"]
    
    E -->|"Route & Enqueue"| F["Message Provider<br/>(In-Memory / RabbitMQ)"]
    
    F -->|"Queue"| G["Message Queue<br/>(Async Processing)"]
    
    G -->|"Consume"| H["CommandHandler<br/>(IConsumer Implementation)"]
    
    H -->|"Process"| I["Business Logic<br/>(Use Cases)"]
    
    I -->|"Send"| J["Outbound<br/>(Internal API / Services)"]
    
    style A fill:#e1f5ff
    style B fill:#fff3e0
    style C fill:#f3e5f5
    style D fill:#e8f5e9
    style E fill:#fce4ec
    style F fill:#fff8e1
    style G fill:#f1f8e9
    style H fill:#e0f2f1
    style I fill:#ede7f6
    style J fill:#e1bee7
```

### Creating New Consumers

To add a new consumer for a command type, implement the MassTransit `IConsumer<T>` interface:

```csharp
// Core/Adapters/In/Messaging/EventHandlers/MyCommandHandler.cs
public class MyCommandHandler(
    IMyUseCase useCase
) : IConsumer<MyCommand> {
    public async Task Consume(ConsumeContext<MyCommand> context)
    {
        // Process the command
        await useCase.ExecuteAsync(context.Message);
    }
}
```

Register the consumer in the MassTransit configuration:

```csharp
// Core/CoreDependencyInjection.cs
services.AddMassTransit(x =>
{
    x.AddConsumer<MyCommandHandler>();
    // ... other consumers
});
```

MassTransit automatically discovers and registers all consumers from assemblies, making new handlers plug-and-play.

---

## 🧪 Testing

### Unit Tests
- **Location**: `Test/Orders.Integrations.Hub.UnitTests/`
- **Framework**: xUnit
- **Coverage**: Business logic, caching, validations, integrations
- **Example**: `Integrations/IntegrationKeyValidationTests.cs`

### Integration Tests (NEW)
- **Location**: `Test/Orders.Integrations.Hub.IntegrationTests/`
- **Framework**: xUnit + **MassTransit TestFramework**
- **Purpose**: Verify command dispatch, message routing, and handler execution
- **Key Test Files**:
  - `CommandHandlers/CommandDispatcherTests.cs` — Direct dispatcher functionality and message routing
  - `CommandHandlers/CreateOrderCommandHandlerTests.cs` — Order creation command flow
  - `CommandHandlers/UpdateOrderCommandHandlerTests.cs` — Order status update scenarios
  - `CommandHandlers/ProcessOrderDisputeCommandHandlerTests.cs` — Dispute handling logic
  - `CommandHandlers/PubSubCommandHandlerTests.cs` — Publish/Subscribe patterns
  - `Extensions/MassTransitTestHarnessExtensions.cs` — Reusable test setup utilities

### Architecture Tests
- **Location**: `Test/Orders.Integrations.Hub.ArchTests/`
- **Framework**: **NetArchTest**
- **Coverage**: 
  - Domain independence from infrastructure
  - Application layer isolation
  - Dependency direction correctness
  - Naming conventions enforcement
  - Serialization rules validation

---

## 🧪 Observability & Monitoring

- **OpenTelemetry**: Tracing, logging, and metrics.
- **Grafana**: Dashboards to visualize telemetry data.
- **Aspire**: Service discovery and metrics aggregation.

---

## 🧰 Local Development

### 🐳 With Docker Compose

```bash
docker-compose up --build -d
```

Includes:
- Orders.Integrations.Hub
- Grafana
- Aspire
- LocalStack (mock Cloud)

### ☁️ Infrastructure

Using Terraform to provision LocalStack:

```bash
cd infra/terraform/envs/local
terraform init
terraform apply
```

Provisioned modules:
- API Gateway
- S3

### 🔧 Configuration

Copy the example env file and fill in the required values:

```bash
cp .env.example .env
```

#### 🧬 Caching Modes

The hub supports three cache modes via `CACHE__MODE` environment variable:

- **Memory** — In-process caching using `MemoryCache`. Suitable for development or single-instance deployments. No external dependencies.
- **Distributed** — Redis-only caching using `StackExchange.Redis`. Use for multi-instance deployments requiring shared cache across instances.
- **Hybrid** — Two-tier caching: in-memory (L1) + Redis (L2). Best for latency-sensitive workloads with cache sharing; reads hit L1 first, misses fetch from L2 (Redis).

**Example:**
```bash
# Development (in-process)
CACHE__MODE=Memory

# Production with Redis
CACHE__MODE=Distributed
CACHE__CONFIGURATIONS__CONNECTION_STRING=redis.example.com:6379,password=secret

# Hybrid (local fast + shared)
CACHE__MODE=Hybrid
CACHE__CONFIGURATIONS__CONNECTION_STRING=redis.example.com:6379
```

For local testing of Distributed/Hybrid modes, bring up Redis via `docker-compose up redis` and use `CACHE__CONFIGURATIONS__CONNECTION_STRING=redis:6379`.

#### 🔑 Environment Variables Overview

> The exact keys used depend on whether an integration is **multi-tenant** or **centralized**.  
> Multi-tenant integrations fetch credentials from the internal API (and mostly need base URLs).  
> Centralized integrations read credentials directly from env vars + cache keys.

| Category   | Variables (examples)                                                                                                                                                                                |
|------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Internal   | `ORDERS__ENDPOINT__BASE_URL` — Internal Orders API endpoint                                                                                                                                         |
| Internal   | `INTERNAL__ENDPOINT__BASE_URL` — Internal API endpoint                                                                                                                                              |
| iFood      | `INTEGRATIONS__IFOOD__ENDPOINT__BASE_URL`, <br>`INTEGRATIONS__IFOOD__CLIENT__ID`, `INTEGRATIONS__IFOOD__CLIENT__SECRET` *(centralized)*                          |
| Rappi      | `INTEGRATIONS__RAPPI__ENDPOINT__BASE_URL`, <br>`INTEGRATIONS__RAPPI__CLIENT__ID`, `INTEGRATIONS__RAPPI__SECRET`, `INTEGRATIONS__RAPPI__AUDIENCE` *(centralized)* |
| 99Food     | `INTEGRATIONS__FOOD99__ENDPOINT__BASE_URL`, <br>`INTEGRATIONS__FOOD99__CLIENT__ID`, `INTEGRATIONS__FOOD99__CLIENT__SECRET` *(centralized)*                      |
| Cache      | `CACHE__MODE` — one of: Memory, Distributed, Hybrid |
| Cache      | `CACHE__CONFIGURATIONS__CONNECTION_STRING` — Redis connection string (e.g., `redis:6379` or `localhost:6379,password=abc123`) |
| Pub/Sub    | `PUB_SUB__TOPICS__ACCEPT_ORDER`, `PUB_SUB__IS_LOCAL`                                                                                                                                                |
| Messaging  | `MASSTRANSIT__TRANSPORT` — Transport mode: `InMemory` (development) or `RabbitMQ` (production)                                                                                                      |
| RabbitMQ   | `RABBITMQ__HOST` — RabbitMQ server hostname (default: `localhost`)                                                                                                                                   |
| RabbitMQ   | `RABBITMQ__PORT` — RabbitMQ port (default: `5672`)                                                                                                                                                  |
| RabbitMQ   | `RABBITMQ__USERNAME` — Authentication username (default: `guest`)                                                                                                                                    |
| RabbitMQ   | `RABBITMQ__PASSWORD` — Authentication password (default: `guest`)                                                                                                                                    |
| RabbitMQ   | `RABBITMQ__VHOST` — Virtual host (default: `/`)                                                                                                                                                     |
| Storage    | `OBJECT_STORAGE__BUCKET__NAME`                                                                                                                                                                      |
| LocalStack | `LOCALSTACK__AWS__IS_LOCALSTACK`, `LOCALSTACK__ENDPOINT_URL`                                                                                                                                        |
| AWS        | `AWS_PROFILE`, `AWS_REGION`                                                                                                                                                                         |
| Telemetry  | `OTEL_SERVICE_NAME`, `OTEL_EXPORTER_OTLP_ENDPOINT`                                                                                                                                                  |

📁 For local development, most values can be left blank or set to local/test equivalents.

#### 🐰 RabbitMQ Configuration (Production)

For production environments using RabbitMQ as the message bus transport:

```bash
# .env or environment variables
MASSTRANSIT__TRANSPORT=RabbitMQ
RABBITMQ__HOST=rabbitmq.yourdomain.com
RABBITMQ__PORT=5672
RABBITMQ__USERNAME=your_username
RABBITMQ__PASSWORD=your_password
RABBITMQ__VHOST=/your_vhost
```

**In-Memory Configuration (Development)**

For local development, MassTransit uses in-memory transport by default:

```bash
MASSTRANSIT__TRANSPORT=InMemory
```

No additional configuration needed — all commands and messages stay in memory for fast iteration.

---

## 📦 Test Dependencies & Installation

Test projects use centralized package management via `Directory.Packages.props` for consistency and easy upgrades.

### Shared Test Packages
File: `Test/Directory.Packages.props`

```xml
<ItemGroup>
  <!-- Testing Framework -->
  <PackageReference Include="xunit" Version="2.x.x" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.x.x" />
  
  <!-- Test Data Generation -->
  <PackageReference Include="Bogus" Version="37.x.x" />
  
  <!-- Assertions -->
  <PackageReference Include="Shouldly" Version="4.x.x" />
  
  <!-- Mocking -->
  <PackageReference Include="NSubstitute" Version="5.x.x" />
</ItemGroup>
```

### Project-Specific Packages

**Integration Tests** (`Orders.Integrations.Hub.IntegrationTests.csproj`)
```xml
<ItemGroup>
  <PackageReference Include="MassTransit.TestFramework" Version="8.x.x" />
  <!-- Inherits all common test packages -->
</ItemGroup>
```

**Architecture Tests** (`Orders.Integrations.Hub.ArchTests.csproj`)
```xml
<ItemGroup>
  <PackageReference Include="NetArchTest.Rules" Version="1.x.x" />
  <!-- Inherits all common test packages -->
</ItemGroup>
```

### Running Tests

```bash
# Restore all dependencies (including test packages)
dotnet restore

# Run all tests in the solution
dotnet test

# Run specific test project
dotnet test Test/Orders.Integrations.Hub.IntegrationTests

# Run specific test with filter
dotnet test --filter "FullyQualifiedName~CommandDispatcherTests"

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

### Adding New Test Projects

When adding a new test project:

1. **Create the project** referencing the test framework
2. **Add project reference** to `Orders.Integrations.Hub.TestCommon` (if using Fakers)
3. **Packages are inherited** from `Directory.Packages.props` — no duplication needed
4. **For integration tests**: Add `MassTransit.TestFramework` to the `.csproj` only if needed

---

## 🔧 Tech Stack

| Layer               | Technology                              |
|---------------------|----------------------------------------|
| Framework           | .NET 10, Minimal API                   |
| Architecture        | Hexagonal (Ports & Adapters)           |
| Docs                | Scalar                                 |
| Caching             | Memory (L1), Redis (L2) — supports Memory-only, Redis Distributed, or Hybrid L1/L2 |
| Multi-Tenancy       | IntegrationContext + Cache             |
| JSON Serialization  | Per-integration serializers            |
| **Messaging**       | **MassTransit (in-memory & RabbitMQ)** |
| Tracing/Telemetry   | OpenTelemetry                          |
| Monitoring          | Grafana, Aspire                        |
| Infra as Code       | Terraform + LocalStack                 |
| Containers          | Docker Compose                         |
| **Message Testing** | **MassTransit.TestFramework**          |
| **Testing**         | **xUnit, NetArchTest, Bogus, Shouldly, NSubstitute** |

---

## 🧠 Future Improvements

- Saga pattern for distributed transactions
- Backpressure with queue-based ingestion
- Circuit breaker patterns with MassTransit policy configuration
- Expand Integration SDK Generator from Core contracts
- Improve observability dashboards per integration with MassTransit metrics
- Distributed tracing across message boundaries

---

## 🤝 Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

---

## 📄 License

[MIT](LICENSE)

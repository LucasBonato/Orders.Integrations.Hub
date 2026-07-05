# builders

**When:** Creating or using test data builders for commands, domain entities, or DTOs in test code.

## Patterns

- **Bogus Fakers** with `Faker<T>` base class and `CustomInstantiator()` for complex domain objects
- **Fluent `.With*()` methods** returning the faker instance for partial customization
- **ObjectMother** for pre-built common objects (see `TestCommon/Fixtures/ObjectMother.cs`)
- **Composition over inheritance** — compose complex Fakers from simpler Fakers (e.g., `CreateOrderCommandFaker` uses `OrderFaker`)

## Examples

| Faker | File Path | What It Builds |
|---|---|---|
| `CreateOrderCommandFaker` | `TestCommon/Fakers/Commands/CreateOrderCommandFaker.cs` | `CreateOrderCommand` with `OrderFaker` |
| `OrderFaker` | `TestCommon/Fakers/Order/OrderFaker.cs` | Full `Order` entity with all optionals |
| `ProcessOrderDisputeCommandFaker` | `TestCommon/Fakers/Commands/ProcessOrderDisputeCommandFaker.cs` | Dispute commands with configurable type/dispute |
| `UpdateOrderStatusCommandFaker` | `TestCommon/Fakers/Commands/UpdateOrderStatusCommandFaker.cs` | Status update commands |
| `SendNotificationCommandFaker` | `TestCommon/Fakers/Commands/SendNotificationCommandFaker.cs` | SNS notification commands |
| `AddressFaker`, `OrderItemFaker`, `OrderPaymentFaker` (et al.) | `TestCommon/Fakers/Order/` | Individual order sub-components |
| `ObjectMother.CreateIntegration()` | `TestCommon/Fixtures/ObjectMother.cs` | Pre-built `Integration` and `IIntegrationContext` |

## Conventions

- **Place all Fakers** in `Test/Orders.Integrations.Hub.TestCommon/Fakers/` under the appropriate subdirectory
- **Name:** `{EntityName}Faker.cs` (e.g., `OrderFaker`, `CreateOrderCommandFaker`)
- **Faker class is `sealed`** extends `Faker<T>` from Bogus
- **Constructor** uses `CustomInstantiator(f => new T(...))` with `faker.Random.*`, `faker.PickRandom<>()`, `faker.Date.*`
- **`.With*()` methods** return `this` for chaining; use `CustomInstantiator(_ => Generate() with { Prop = value })`
- **Provide `.WithoutOptionals()`** method for entities with many nullable fields (see `OrderFaker`)
- **ObjectMother** is static, not a Faker — use for simple objects where randomization isn't needed
- **Fakers compose** — `CreateOrderCommandFaker` calls `new OrderFaker().Generate()`, not individual field fakers

## Anti-Patterns

- ❌ **Putting Fakers in test project** — they should be in `TestCommon` so all test projects can use them
- ❌ **Inline builder methods in test classes** — extract to a Faker when used in more than one test
- ❌ **Overriding Bogus with deterministic values in Faker constructors** — use `.With*()` for test-specific overrides, keep defaults random
- ❌ **Fakers that produce invalid domain objects** — ensure required fields are always populated, use `faker.Random.*` for valid ranges
- ❌ **Duplicating field mappings** — if `OrderFaker` changes, update its constructor, not every test that sets those fields inline

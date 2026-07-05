# Test Data Builders

This repo uses the **Builder pattern** via **Bogus Faker** for test data creation. Builders live in `Test/Orders.Integrations.Hub.TestCommon/Fakers/` and generate realistic, randomized domain objects.

## Bogus Faker Usage

Each builder extends `Faker<T>` and uses `CustomInstantiator` to construct the object with randomized data:

```csharp
// From CreateOrderCommandFaker
public sealed class CreateOrderCommandFaker : Faker<CreateOrderCommand>
{
    public CreateOrderCommandFaker()
    {
        CustomInstantiator(_ => new CreateOrderCommand(
            Order: new OrderFaker().Generate()
        ));
    }
}
```

Deeper builders compose other builders — `OrderFaker` uses `OrderMerchantFaker`, `OrderItemFaker`, `OrderDiscountFaker`, etc.

## Fluent Builder Methods

Builders expose `.With*()` methods for overriding specific properties:

```csharp
// From OrderFaker
public OrderFaker WithSalesChannel(string salesChannel) {
    CustomInstantiator(_ => Generate() with { SalesChannel = salesChannel });
    return this;
}

public OrderFaker WithoutOptionals() {
    CustomInstantiator(_ => Generate() with {
        Dispute = null, Delivery = null, Takeout = null,
        Indoor = null, Customer = null, Schedule = null,
        OrderPriority = null, Payments = null, TaxInvoice = null
    });
    return this;
}
```

Usage:

```csharp
Order order = new OrderFaker()
    .WithSalesChannel("RAPPI")
    .WithType(OrderType.DELIVERY)
    .WithoutOptionals()
    .Generate();

SendNotificationCommand cmd = new SendNotificationCommandFaker()
    .WithTopicArn("arn:aws:sns:us-east-1:123456789012:test-topic")
    .Generate();
```

## When to Create a Builder vs Using ObjectMother

| Situation | Approach |
|---|---|
| Need a random valid object with all fields populated | `new OrderFaker().Generate()` |
| Need an object with specific overrides | `new OrderFaker().WithSalesChannel("RAPPI").Generate()` |
| Need a simple, deterministic object | `ObjectMother.CreateIntegration()` |
| Test requires an exact control value | `ObjectMother.CreateIntegration(merchantId: "fixed-id")` |
| Creating a new domain aggregate or DTO | Create a new Faker in `TestCommon/Fakers/` |
| Rare one-off scenario in a single test file | Inline constructor call |

**Guideline**: If the type is reused across test files, add a Faker. If it's used once, inline it. If it's a common configuration (like `Integration`), use `ObjectMother`.

## Creating a New Builder

1. Create a file in `Test/Orders.Integrations.Hub.TestCommon/Fakers/{Category}/`
2. Extend `Faker<T>` where T is the type to build
3. Use `CustomInstantiator` in the constructor
4. Add `.With*()` methods for commonly overridden properties
5. Reference the faker's `Generate()` method in parent fakers

using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderFaker : Faker<Core.Domain.Entity.Order> {
    public OrderFaker() {
        CustomInstantiator(faker => new Core.Domain.Entity.Order(
            OrderId: faker.Random.Guid().ToString(),
            Type: faker.PickRandom<OrderType>(),
            DisplayId: faker.Random.AlphaNumeric(8).ToUpper(),
            SourceAppId: faker.Random.Guid().ToString(),
            SalesChannel: faker.PickRandom("ifood", "rappi", "food99"),
            VirtualBrand: faker.Random.Bool() ? faker.Company.CompanyName() : null,
            CreatedAt: faker.Date.Recent(),
            LastEvent: faker.PickRandom<OrderEventType>(),
            OrderTiming: faker.PickRandom<OrderTiming>(),
            PreparationStartDateTime: faker.Date.Soon(),
            Merchant: new OrderMerchantFaker().Generate(),
            Items: new OrderItemFaker().Generate(faker.Random.Int(1, 5)),
            OtherFees: new OrderFeeFaker().Generate(faker.Random.Int(0, 2)),
            Discounts: new OrderDiscountFaker().Generate(faker.Random.Int(0, 2)),
            Total: new OrderTotalFaker().Generate(),
            Payments: faker.Random.Bool() ? new OrderPaymentFaker().Generate() : null,
            TaxInvoice: faker.Random.Bool() ? new OrderTaxInvoiceFaker().Generate() : null,
            Customer: faker.Random.Bool() ? new OrderCustomerFaker().Generate() : null,
            Schedule: faker.Random.Bool() ? new OrderScheduleFaker().Generate() : null,
            OrderPriority: faker.Random.Bool() ? faker.PickRandom<OrderPriority>() : null,
            Delivery: faker.Random.Bool() ? new OrderDeliveryFaker().Generate() : null,
            Takeout: faker.Random.Bool() ? new OrderTakeoutFaker().Generate() : null,
            Indoor: faker.Random.Bool() ? new OrderIndoorFaker().Generate() : null,
            SendPreparing: faker.Random.Bool(),
            SendDelivered: faker.Random.Bool(),
            SendPickedUp: faker.Random.Bool(),
            SendTracking: faker.Random.Bool(),
            ExtraInfo: "{}",
            ExternalId: faker.Random.Guid().ToString(),
            OrderDisplayId: faker.Random.AlphaNumeric(6).ToUpper(),
            TenantId: faker.Random.Guid().ToString(),
            Dispute: null
        ));
    }

    public OrderFaker WithSalesChannel(string salesChannel) {
        CustomInstantiator(_ => Generate() with { SalesChannel = salesChannel });
        return this;
    }

    public OrderFaker WithType(OrderType type) {
        CustomInstantiator(_ => Generate() with { Type = type });
        return this;
    }

    public OrderFaker WithDispute(OrderDispute? dispute = null) {
        CustomInstantiator(_ => Generate() with {
            Dispute = dispute ?? new OrderDisputeFaker().Generate()
        });
        return this;
    }

    public OrderFaker WithDelivery(OrderDelivery? delivery = null) {
        CustomInstantiator(_ => Generate() with {
            Delivery = delivery ?? new OrderDeliveryFaker().Generate()
        });
        return this;
    }

    public OrderFaker WithoutOptionals() {
        CustomInstantiator(_ => Generate() with {
            Dispute = null,
            Delivery = null,
            Takeout = null,
            Indoor = null,
            Customer = null,
            Schedule = null,
            OrderPriority = null,
            Payments = null,
            TaxInvoice = null
        });
        return this;
    }
}
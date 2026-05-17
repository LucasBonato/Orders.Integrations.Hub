using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderTotalFaker : Faker<OrderTotal> {
    public OrderTotalFaker() {
        CustomInstantiator(faker => new OrderTotal(
            ItemsPrice: new PriceFaker().Generate(),
            OtherFees: new PriceFaker().Generate(),
            Discount: faker.Random.Bool() ? new PriceFaker().Generate() : null,
            OrderAmount: new PriceFaker().Generate()
        ));
    }
}
using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Discount;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderDiscountFaker : Faker<OrderDiscount> {
    public OrderDiscountFaker() {
        CustomInstantiator(faker => new OrderDiscount(
            Amount: new PriceFaker().Generate(),
            Target: faker.PickRandom<DiscountTarget>(),
            TargetId: faker.Random.Guid().ToString(),
            SponsorshipValues: new OrderDiscountSponsorshipValueFaker().Generate(faker.Random.Int(1, 3))
        ));
    }
}
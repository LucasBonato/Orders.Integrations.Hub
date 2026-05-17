using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Discount;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderDiscountSponsorshipValueFaker : Faker<OrderDiscountSponsorshipValue> {
    public OrderDiscountSponsorshipValueFaker() {
        CustomInstantiator(faker => new OrderDiscountSponsorshipValue(
            Name: faker.PickRandom<OrderSponsorshipName>(),
            Amount: new PriceFaker().Generate(),
            Description: faker.Random.Bool() ? faker.Lorem.Sentence() : null
        ));
    }
}
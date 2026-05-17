using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class PriceFaker : Faker<Price> {
    public PriceFaker() {
        CustomInstantiator(faker => new Price(
            Value: faker.Random.Decimal(0.01m, 999.99m),
            Currency: "BRL"
        ));
    }
}
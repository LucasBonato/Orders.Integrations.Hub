using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderFeeFaker : Faker<OrderFee> {
    public OrderFeeFaker() {
        CustomInstantiator(faker => new OrderFee(
            Name: faker.PickRandom("Delivery Fee", "Service Fee", "Tip"),
            Type: faker.PickRandom<FeeType>(),
            ReceivedBy: faker.PickRandom<FeeReceivedBy>(),
            Price: new PriceFaker().Generate(),
            ReceiverDocument: faker.Random.Replace("##.###.###/####-##"),
            Observation: faker.Random.Bool() ? faker.Lorem.Sentence() : string.Empty
        ));
    }
}
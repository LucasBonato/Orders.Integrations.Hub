using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderIndoorFaker : Faker<OrderIndoor> {
    public OrderIndoorFaker() {
        CustomInstantiator(faker => new OrderIndoor(
            Mode: faker.PickRandom<OrderIndoorMode>(),
            IndoorDateTime: faker.Date.Soon(),
            Place: faker.Random.Bool() ? faker.Random.AlphaNumeric(4).ToUpper() : null,
            Seat: faker.Random.Bool() ? faker.Random.Int(1, 50).ToString() : null,
            Tab: faker.Random.Bool() ? faker.Random.AlphaNumeric(6).ToUpper() : null
        ));
    }
}
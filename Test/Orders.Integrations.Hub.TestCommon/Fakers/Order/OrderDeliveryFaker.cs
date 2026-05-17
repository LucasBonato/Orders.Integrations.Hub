using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderDeliveryFaker : Faker<OrderDelivery> {
    public OrderDeliveryFaker() {
        CustomInstantiator(faker => new OrderDelivery(
            DeliveredBy: faker.PickRandom<OrderDeliveredBy>(),
            EstimatedDeliveryDateTime: faker.Date.Soon(),
            DeliveryDateTime: faker.Date.Soon(),
            DeliveryAddress: faker.Random.Bool() ? new AddressFaker().Generate() : null,
            PickupCode: faker.Random.Bool() ? faker.Random.AlphaNumeric(6).ToUpper() : null
        ));
    }
}
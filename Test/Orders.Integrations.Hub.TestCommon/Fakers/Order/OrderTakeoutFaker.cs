using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderTakeoutFaker : Faker<OrderTakeout> {
    public OrderTakeoutFaker() {
        CustomInstantiator(faker => new OrderTakeout(
            Mode: faker.PickRandom<OrderTakeoutMode>(),
            TakeoutDateTime: faker.Date.Soon()
        ));
    }
}
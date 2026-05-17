using Bogus;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.TestCommon.Fakers.Order;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Commands;

public sealed class CreateOrderCommandFaker : Faker<CreateOrderCommand> {
    public CreateOrderCommandFaker() {
        CustomInstantiator(_ => new CreateOrderCommand(
            Order: new OrderFaker().Generate()
        ));
    }

    public CreateOrderCommandFaker WithOrder(Core.Domain.Entity.Order order) {
        CustomInstantiator(_ => new CreateOrderCommand(Order: order));
        return this;
    }
}
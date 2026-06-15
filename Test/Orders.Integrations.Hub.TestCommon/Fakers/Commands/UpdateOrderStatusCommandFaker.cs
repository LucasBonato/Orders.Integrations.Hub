using Bogus;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Commands;

public sealed class UpdateOrderStatusCommandFaker : Faker<UpdateOrderStatusCommand> {
    public UpdateOrderStatusCommandFaker() {
        CustomInstantiator(faker => new UpdateOrderStatusCommand(
            OrderUpdate: new OrderUpdateFaker().Generate(),
            SalesChannel: IntegrationKey.From(faker.PickRandom("ifood", "rappi", "food99"))
        ));
    }

    public UpdateOrderStatusCommandFaker WithOrderUpdate(OrderUpdate orderUpdate) {
        CustomInstantiator(faker => new UpdateOrderStatusCommand(
            OrderUpdate: orderUpdate,
            SalesChannel: IntegrationKey.From(faker.PickRandom("ifood", "rappi", "food99"))
        ));
        return this;
    }
}
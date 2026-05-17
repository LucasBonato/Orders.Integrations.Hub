using Bogus;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.TestCommon.Fakers.Order;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Commands;

public sealed class ProcessOrderDisputeCommandFaker : Faker<ProcessOrderDisputeCommand> {
    private OrderEventType? _type;
    private OrderDispute? _dispute;
    private bool _withDispute;

    public ProcessOrderDisputeCommandFaker() {
        CustomInstantiator(faker => new ProcessOrderDisputeCommand(
            ExternalOrderId: faker.Random.Guid().ToString(),
            Integration: IntegrationKey.From(faker.PickRandom("ifood", "rappi", "food99")),
            OrderDispute: _withDispute
                ? _dispute ?? new OrderDisputeFaker().Generate()
                : null,
            Type: _type ?? faker.PickRandom<OrderEventType>()
        ));
    }

    public ProcessOrderDisputeCommandFaker WithDispute(OrderDispute? dispute = null) {
        _withDispute = true;
        _dispute = dispute;
        return this;
    }

    public ProcessOrderDisputeCommandFaker WithType(OrderEventType type) {
        _type = type;
        return this;
    }
}
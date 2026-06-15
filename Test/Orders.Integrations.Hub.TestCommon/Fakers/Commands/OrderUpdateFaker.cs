using Bogus;

using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.TestCommon.Fakers.Order;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Commands;

public sealed class OrderUpdateFaker : Faker<OrderUpdate> {
    private OrderDispute? _dispute;
    private bool _withDispute;
    private OrderEventType? _type;

    public OrderUpdateFaker() {
        CustomInstantiator(f => new OrderUpdate(
            OrderId: f.Random.Guid().ToString(),
            SourceAppId: f.Random.Guid().ToString(),
            Type: _type ?? f.PickRandom<OrderEventType>(),
            CreateAt: f.Date.Recent(),
            Dispute: _withDispute
                ? _dispute ?? new OrderDisputeFaker().Generate()
                : null,
            FromIntegration: f.Random.Bool()
        ));
    }

    public OrderUpdateFaker WithDispute(OrderDispute? dispute = null) {
        _withDispute = true;
        _dispute = dispute;
        return this;
    }

    public OrderUpdateFaker WithType(OrderEventType type) {
        _type = type;
        return this;
    }
}
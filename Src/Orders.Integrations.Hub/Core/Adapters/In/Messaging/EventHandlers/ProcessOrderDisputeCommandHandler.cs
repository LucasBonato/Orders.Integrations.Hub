using MassTransit;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public sealed class ProcessOrderDisputeCommandHandler(
    IOrderClient orderClient
) : IConsumer<ProcessOrderDisputeCommand> {
    public async Task Consume(ConsumeContext<ProcessOrderDisputeCommand> context)
    {
        ProcessOrderDisputeCommand command = context.Message;

        OrderUpdate orderUpdate = new(
            OrderId: command.ExternalOrderId,
            SourceAppId: command.Integration,
            Type: command.Type,
            CreateAt: DateTime.UtcNow,
            Dispute: command.OrderDispute,
            FromIntegration: !command.Integration.Equals(IntegrationKey.Nothing())
        );

        await orderClient.PatchOrderDispute(orderUpdate);
    }
}
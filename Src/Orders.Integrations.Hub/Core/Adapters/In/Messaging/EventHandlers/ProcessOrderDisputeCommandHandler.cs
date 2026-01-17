using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class ProcessOrderDisputeCommandHandler(
    IOrderClient orderClient
) : IEventHandler<FastEndpointsCommandEnvelope<ProcessOrderDisputeCommand>> {
    public async Task HandleAsync(FastEndpointsCommandEnvelope<ProcessOrderDisputeCommand> envelope, CancellationToken ct)
    {
        ProcessOrderDisputeCommand command = envelope.Command;

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
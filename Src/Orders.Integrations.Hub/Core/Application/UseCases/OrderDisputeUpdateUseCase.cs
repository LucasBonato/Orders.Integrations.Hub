using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Application.UseCases;

public class OrderDisputeUpdateUseCase(
    IOrderClient orderClient
) : IOrderDisputeUpdateUseCase {
    public async Task ProcessDispute(ProcessOrderDisputeCommand orderDisputeCommand) {
        OrderUpdate orderUpdate = new(
            OrderId: orderDisputeCommand.ExternalOrderId,
            SourceAppId: orderDisputeCommand.Integration,
            Type: orderDisputeCommand.Type,
            CreateAt: DateTime.UtcNow,
            Dispute: orderDisputeCommand.OrderDispute,
            FromIntegration: true
        );

        await orderClient.PatchOrderDispute(orderUpdate);
    }
}
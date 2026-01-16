using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Application.UseCases;

public class OrderDisputeUpdateUseCase(
    IOrderClient orderClient
) : IOrderDisputeUpdateUseCase {
    public async Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent) {
        OrderUpdate orderUpdate = new(
            OrderId: orderDisputeEvent.ExternalOrderId,
            SourceAppId: orderDisputeEvent.Integration,
            Type: orderDisputeEvent.Type,
            CreateAt: DateTime.UtcNow,
            Dispute: orderDisputeEvent.OrderDispute,
            FromIntegration: true
        );

        await orderClient.PatchOrderDispute(orderUpdate);
    }
}
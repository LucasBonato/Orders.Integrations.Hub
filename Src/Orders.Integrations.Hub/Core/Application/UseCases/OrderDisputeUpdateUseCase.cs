using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Core;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;

namespace Orders.Integrations.Hub.Core.Application.UseCases;

public class OrderDisputeUpdateUseCase(
    IOrderClient orderClient
) : IOrderDisputeUpdateUseCase {
    public async Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent)
    {
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
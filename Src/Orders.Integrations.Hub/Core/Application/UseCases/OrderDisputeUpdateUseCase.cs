using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Application.UseCases;

public class OrderDisputeUpdateUseCase(
    IOrderClient orderClient
) : IOrderDisputeUpdateUseCase {
    public async Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent)
    {
        OrderUpdate orderUpdate = new(
            OrderId: orderDisputeEvent.ExternalOrderId,
            SourceAppId: orderDisputeEvent.Integration.ToString(),
            Type: orderDisputeEvent.Type,
            CreateAt: DateTime.UtcNow,
            Dispute: orderDisputeEvent.OrderDispute,
            FromIntegration: true
        );

        await orderClient.PatchOrderDispute(orderUpdate);
    }
}
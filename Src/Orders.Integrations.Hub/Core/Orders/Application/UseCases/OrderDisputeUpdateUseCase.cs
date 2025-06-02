using Orders.Integrations.Hub.Core.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Orders.Application.UseCases;

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
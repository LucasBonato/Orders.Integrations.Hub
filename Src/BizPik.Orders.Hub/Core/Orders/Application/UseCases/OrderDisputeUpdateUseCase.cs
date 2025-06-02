using BizPik.Orders.Hub.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

namespace BizPik.Orders.Hub.Core.Orders.Application.UseCases;

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
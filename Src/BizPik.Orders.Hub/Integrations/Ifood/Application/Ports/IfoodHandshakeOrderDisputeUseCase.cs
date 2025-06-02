using System.Text.Json;

using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Ports;

public class IfoodHandshakeOrderDisputeUseCase : IOrderDisputeUseCase<IfoodWebhookRequest>
{
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest ifoodOrder)
    {
        OrderEventType type = ifoodOrder.FullCode is IfoodFullOrderStatus.HANDSHAKE_DISPUTE
            ? OrderEventType.DISPUTE_STARTED
            : OrderEventType.DISPUTE_FINISH;

        HandshakeDispute? dispute = null;

        if (type == OrderEventType.DISPUTE_STARTED)
        {
            string json = JsonSerializer.Serialize(ifoodOrder.Metadata);

            dispute = JsonSerializer.Deserialize<HandshakeDispute>(json)
                                       ?? throw new Exception("Não foi possível converter disputa!");
        }


        await new ProcessOrderDisputeEvent(
            ExternalOrderId: ifoodOrder.OrderId,
            Integration: OrderIntegration.IFOOD,
            OrderDispute: dispute?.ToOrder(),
            Type: type
        ).PublishAsync();

        return ifoodOrder;
    }
}
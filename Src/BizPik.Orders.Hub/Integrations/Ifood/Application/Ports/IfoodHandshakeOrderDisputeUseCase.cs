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

        string json = JsonSerializer.Serialize(ifoodOrder.Metadata);

        if (type == OrderEventType.DISPUTE_STARTED) {
            dispute = JsonSerializer.Deserialize<HandshakeDispute>(json);
        }
        else {
            HandshakeSettlement? settlement = JsonSerializer.Deserialize<HandshakeSettlement>(json);
            if (settlement != null)
            {
                dispute = new HandshakeDispute(
                    DisputeId: settlement.DisputeId,
                    ParentDisputeId: settlement.DisputeId,
                    Message: settlement.Reason?? settlement.Status.ToString(),
                    Alternatives: settlement.SelectedDisputeAlternative is { } alternative
                        ? [
                            new DisputeAlternative(
                                Id: alternative.Id,
                                Type: alternative.Type,
                                Metadata: alternative.Metadata,
                                Amount: alternative.Metadata.Amount,
                                AllowedsAdditionalTimeInMinutes: alternative.Metadata.AllowedsAdditionalTimeInMinutes,
                                AllowedsAdditionalTimeReasons: alternative.Metadata.AllowedsAdditionalTimeReasons
                            )
                        ]
                        : null,
                    Action: default,
                    TimeoutAction: default,
                    ExpiresAt: default,
                    CreatedAt: settlement.CreatedAt,
                    HandshakeType: default,
                    HandshakeGroup: default,
                    Metadata: null
                );
            }
        }

        if (dispute is null)
            throw new Exception("Não foi possível converter disputa!");

        await new ProcessOrderDisputeEvent(
            ExternalOrderId: ifoodOrder.OrderId,
            Integration: OrderIntegration.IFOOD,
            OrderDispute: dispute.ToOrder(),
            Type: type
        ).PublishAsync();

        return ifoodOrder;
    }
}
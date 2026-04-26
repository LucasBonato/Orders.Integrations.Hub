using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;

public class IFoodHandshakeOrderDisputeUseCase(
    [FromKeyedServices(IFoodIntegrationKey.Value)] IOrderDisputeEvidenceStorage<Media> evidenceStorage,
    [FromKeyedServices(IFoodIntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    ICommandDispatcher dispatcher
) : IOrderDisputeUseCase<IFoodWebhookRequest> {
    public async Task<IFoodWebhookRequest> ExecuteAsync(IFoodWebhookRequest ifoodOrder)
    {
        OrderEventType type = ifoodOrder.FullCode is IFoodFullOrderStatus.HANDSHAKE_DISPUTE
            ? OrderEventType.DISPUTE_STARTED
            : OrderEventType.DISPUTE_FINISH;
        
        HandshakeDispute dispute = type is OrderEventType.DISPUTE_STARTED
            ? await HandleDisputeStarted(ifoodOrder)
            : await HandleDisputeFinished(ifoodOrder);

        await dispatcher.DispatchAsync(
            new ProcessOrderDisputeCommand(
                ExternalOrderId: ifoodOrder.OrderId,
                Integration: IFoodIntegrationKey.IFOOD,
                OrderDispute: dispute.ToOrder(),
                Type: type
            )
        );

        return ifoodOrder;
    }
    
    private async Task<HandshakeDispute> HandleDisputeStarted(IFoodWebhookRequest ifoodOrder)
    {
        string json = jsonSerializer.Serialize(ifoodOrder.Metadata);
        HandshakeDispute dispute = jsonSerializer.Deserialize<HandshakeDispute>(json)
                                   ?? throw new InvalidOperationException("Could not deserialize dispute metadata");

        if (dispute.Metadata?.Evidences is not null) {
            List<DisputeEvidence> disputeEvidences = await evidenceStorage.MigrateEvidencesToStorage(
                orderId: ifoodOrder.OrderId,
                disputeId: dispute.DisputeId,
                evidences: dispute.Metadata.Evidences
            );
            dispute.Metadata.Evidences = disputeEvidences
                .Select(evidence => new Media(evidence.Key, evidence.ContentType))
                .ToList();
        }

        return dispute;
    }

    private async Task<HandshakeDispute> HandleDisputeFinished(IFoodWebhookRequest ifoodOrder) {
        string json = jsonSerializer.Serialize(ifoodOrder.Metadata);
        HandshakeSettlement settlement = jsonSerializer.Deserialize<HandshakeSettlement>(json) 
            ?? throw new InvalidOperationException("Could not deserialize dispute metadata");
        
        await evidenceStorage.DeleteDisputeEvidence(ifoodOrder.OrderId, settlement.DisputeId);
        
        return new HandshakeDispute(
            DisputeId: settlement.DisputeId,
            ParentDisputeId: settlement.DisputeId,
            Message: settlement.Status.ToString(), //?? settlement.Reason?? string.Empty,
            Alternatives: settlement.SelectedDisputeAlternative is { } alternative
                ? [
                    new IFoodDisputeAlternative(
                        Id: alternative.Id,
                        Type: alternative.Type,
                        Metadata: alternative.Metadata,
                        MaxAmount: alternative.Metadata.MaxAmount,
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
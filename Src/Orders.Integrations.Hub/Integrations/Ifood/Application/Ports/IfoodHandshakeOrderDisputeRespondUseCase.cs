using System.Text.Json;

using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports;

public class IfoodHandshakeOrderDisputeRespondUseCase(
    IIFoodClient ifoodClient
) : IOrderDisputeRespondUseCase {
    public async Task ExecuteAsync(RespondDisputeIntegrationRequest respondRequest)
    {
        return respondRequest.Type switch {
            DisputeResponseType.ACCEPT => ifoodClient.PostHandshakeDisputesAccept(disputeId: respondRequest.DisputeId),
            DisputeResponseType.REJECT => ifoodClient.PostHandshakeDisputesReject(disputeId: respondRequest.DisputeId),
            DisputeResponseType.COUNTERPROPOSAL => ifoodClient.PostHandshakeDisputesAlternatives(
                disputeId: respondRequest.DisputeId,
                alternativeId: respondRequest.AlternativeId!,
                request: new HandshakeAlternativeRequest(
                    Metadata: JsonSerializer.Deserialize<HandshakeAlternativeMetadata>(JsonSerializer.Serialize(respondRequest.Metadata))?? throw new Exception(),
                    Type:
                )
            ),
            _ => throw new NotImplementedException(),
        };
    }
}
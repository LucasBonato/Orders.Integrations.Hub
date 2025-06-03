using System.Text.Json;

using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Ports;

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
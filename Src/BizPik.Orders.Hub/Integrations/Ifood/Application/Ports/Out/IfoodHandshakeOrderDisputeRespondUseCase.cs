using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Ports.Out;

public class IfoodHandshakeOrderDisputeRespondUseCase(
    IIFoodClient ifoodClient
) : IOrderDisputeRespondUseCase {
    public async Task ExecuteAsync(RespondDisputeIntegrationRequest respondRequest)
    {
        switch (respondRequest.Type) {
            case DisputeResponseType.ACCEPT:
                await ifoodClient.PostHandshakeDisputesAccept(
                    disputeId: respondRequest.DisputeId,
                    request: respondRequest.DisputeResponse
                );
                break;
            case DisputeResponseType.REJECT:
                await ifoodClient.PostHandshakeDisputesReject(
                    disputeId: respondRequest.DisputeId,
                    request: respondRequest.DisputeResponse
                );
                break;
            case DisputeResponseType.COUNTER_OFFER:
                if (respondRequest.AlternativeId is null)
                    throw new ArgumentNullException(respondRequest.AlternativeId);
                await ifoodClient.PostHandshakeDisputesAlternatives(
                    disputeId: respondRequest.DisputeId,
                    alternativeId: respondRequest.AlternativeId,
                    request: new HandshakeAlternativeRequest(
                        Metadata: new HandshakeAlternativeMetadata(
                            Amount: respondRequest.DisputeResponse.Price?.ToAmount(),
                            AdditionalTimeInMinutes: respondRequest.DisputeResponse.AdditionalTimeInMinutes,
                            AdditionalTimeReason: respondRequest.DisputeResponse.AdditionalTimeReason
                        ),
                        Type: Enum.Parse<HandshakeAlternativeType>(respondRequest.DisputeResponse.Type!)
                    )
                );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
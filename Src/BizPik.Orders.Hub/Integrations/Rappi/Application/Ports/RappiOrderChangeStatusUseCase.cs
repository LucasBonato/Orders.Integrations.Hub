using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Rappi.Application.Ports;

public class RappiOrderChangeStatusUseCase(
    ILogger<RappiOrderChangeStatusUseCase> logger,
    IRappiClient rappiClient
) : IOrderChangeStatusUseCase {
    public async Task ExecuteAsync(ChangeOrderStatusRequest request)
    {
        logger.LogInformation("[INFO] {status} Order [{orderId}] with external id [{orderExternalId}]", request.Status, request.OrderId, request.ExternalId);
        switch (request.Status) {
            case OrderEventType.CONFIRMED:
                await rappiClient.ConfirmOrder(request.ExternalId, request.MerchantId);
                break;
            case OrderEventType.READY_FOR_PICKUP:
                await rappiClient.ReadyToPickupOrder(request.ExternalId);
                break;
            case OrderEventType.CANCELLED:
            case OrderEventType.CANCELLATION_REQUESTED:
            case OrderEventType.ORDER_CANCELLATION_REQUEST:
                RappiOrderCancelType cancelType = (RappiOrderCancelType)Convert.ToInt32(request.CancellationReason);
                await rappiClient.RequestOrderCancellation(
                    request.ExternalId,
                    new RappiOrderRejectRequest(
                        request.CancellationMetadata?.ToString()?? cancelType.ToString(),
                        null,
                        null,
                        cancelType
                    )
                );
                break;
        }
    }
}
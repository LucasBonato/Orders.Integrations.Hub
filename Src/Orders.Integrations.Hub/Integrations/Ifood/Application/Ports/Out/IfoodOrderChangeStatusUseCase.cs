using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports.Out;

public class IfoodOrderChangeStatusUseCase(
    ILogger<IfoodOrderChangeStatusUseCase> logger,
    IIFoodClient iFoodClient
) : IOrderChangeStatusUseCase {
    public async Task ExecuteAsync(ChangeOrderStatusRequest request) {
        logger.LogInformation("[INFO] {status} Order [{orderId}] with external id [{orderExternalId}]", request.Status, request.OrderId, request.ExternalId);
        switch (request.Status) {
            case OrderEventType.CONFIRMED:
                await iFoodClient.ConfirmOrder(request.ExternalId);
                break;
            case OrderEventType.PREPARING:
                await iFoodClient.PreparationStartedOrder(request.ExternalId);
                break;
            case OrderEventType.READY_FOR_PICKUP:
                await iFoodClient.ReadyToPickupOrder(request.ExternalId);
                break;
            case OrderEventType.DISPATCHED:
                await iFoodClient.DispatchOrder(request.ExternalId);
                break;
            case OrderEventType.CANCELLED:
            case OrderEventType.CANCELLATION_REQUESTED:
            case OrderEventType.ORDER_CANCELLATION_REQUEST:
                await iFoodClient.RequestOrderCancellation(
                    request.ExternalId,
                    new IfoodOrderCancellationRequest(request.CancellationReason!)
                );
                break;
        }
    }
}
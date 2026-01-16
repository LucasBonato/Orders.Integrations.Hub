using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.Out;

public class IFoodOrderChangeStatusUseCase(
    ILogger<IFoodOrderChangeStatusUseCase> logger,
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
                    new IFoodOrderCancellationRequest(request.CancellationReason!)
                );
                break;
        }
    }
}
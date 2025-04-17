using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderChangeStatusUseCase(
    ILogger<IfoodOrderChangeStatusUseCase> logger,
    IIFoodClient iFoodClient
) : IOrderChangeStatusUseCase {
    public OrderIntegration Integration => OrderIntegration.IFOOD;

    public async Task Execute(ChangeOrderStatusRequest request) {
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
                await iFoodClient.RequestOrderCancellation(
                    request.ExternalId,
                    new IfoodOrderCancellationRequest(request.CancellationReason!)
                );
                break;
        }
    }
}
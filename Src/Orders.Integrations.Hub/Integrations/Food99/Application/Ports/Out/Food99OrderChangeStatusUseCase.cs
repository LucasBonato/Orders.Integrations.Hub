using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Application.Clients;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.Out;

public class Food99OrderChangeStatusUseCase(
    ILogger<Food99OrderChangeStatusUseCase> logger,
    IIntegrationContext integrationContext,
    IFood99Client food99Client
) : IOrderChangeStatusUseCase {
    public async Task ExecuteAsync(ChangeOrderStatusRequest request)
    {
        integrationContext.MerchantId = request.MerchantId;

        logger.LogInformation(
            "[INFO] {status} ChangeStatusOrder [{orderId}] with external id [{orderExternalId}]",
            request.Status,
            request.OrderId,
            request.ExternalId
        );

        Task changeStatusTask = (request.Status) switch {
            OrderEventType.CONFIRMED => food99Client.ConfirmOrder(
                new Food99StatusChangeRequest(request.ExternalId, null, null, null)
            ),

            OrderEventType.DISPATCHED or
            OrderEventType.READY_FOR_PICKUP => food99Client.ReadyToPickupOrder(
                new Food99StatusChangeRequest(request.ExternalId, null, null, null)
            ),

            OrderEventType.DELIVERED or
            OrderEventType.CONCLUDED => food99Client.DeliveredOrder(
                new Food99StatusChangeRequest(request.ExternalId, null, null, null)
            ),

            OrderEventType.CANCELLED or
            OrderEventType.CANCELLATION_REQUESTED or
            OrderEventType.ORDER_CANCELLATION_REQUEST => food99Client.CancelOrder(
                new Food99StatusChangeRequest(
                    OrderId: request.ExternalId,
                    AuthToken: null,
                    ReasonId: (int)Enum.Parse<Food99OrderCancelType>(request.CancellationReason!),
                    Reason: request.CancellationReason!
                )
            ),

            _ => throw new ArgumentOutOfRangeException()
        };

        await changeStatusTask;
    }
}

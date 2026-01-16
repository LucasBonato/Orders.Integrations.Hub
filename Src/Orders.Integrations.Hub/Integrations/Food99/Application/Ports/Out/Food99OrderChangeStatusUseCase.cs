using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.Out;

public class Food99OrderChangeStatusUseCase(
    ILogger<Food99OrderChangeStatusUseCase> logger,
    IIntegrationContext integrationContext,
    IFood99Client food99Client
) : IOrderChangeStatusUseCase {
    public async Task ExecuteAsync(ChangeOrderStatusRequest request)
    {
        // integrationContext.Integration = await internalClient.GetIntegrationByExternalId(request.MerchantId);
        integrationContext.MerchantId = request.MerchantId;

        logger.LogInformation(
            "[INFO] {status} ChangeStatusOrder [{orderId}] with external id [{orderExternalId}]",
            request.Status,
            request.OrderId,
            request.ExternalId
        );

        Task changeStatusTask = (request.Status) switch {
            OrderEventType.CONFIRMED => food99Client.ConfirmOrder(integrationContext.MerchantId, request.ExternalId),

            OrderEventType.DISPATCHED or
            OrderEventType.READY_FOR_PICKUP => food99Client.ReadyToPickupOrder(integrationContext.MerchantId, request.ExternalId),

            OrderEventType.DELIVERED or
            OrderEventType.CONCLUDED => food99Client.DeliveredOrder(integrationContext.MerchantId, request.ExternalId),

            OrderEventType.CANCELLED or
            OrderEventType.CANCELLATION_REQUESTED or
            OrderEventType.ORDER_CANCELLATION_REQUEST => food99Client.CancelOrder(
                appShopId: integrationContext.MerchantId,
                orderId: request.ExternalId,
                reason: request.CancellationReason!,
                reasonId: (int)Enum.Parse<Food99OrderCancelType>(request.CancellationReason!)
            ),

            _ => throw new ArgumentOutOfRangeException()
        };

        await changeStatusTask;
    }
}
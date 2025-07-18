﻿using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.Out;

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
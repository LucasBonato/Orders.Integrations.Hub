using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.MerchantDetails;
using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

using Refit;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Clients;

public interface IIFoodClient : IIntegrationClient {
    [Get("/order/v1.0/orders/{orderId}")]
    Task<IFoodOrder> GetOrderDetails(string orderId);

    [Get("/merchant/v1.0/merchants/{merchantId}")]
    Task<IFoodMerchant> GetMerchantDetails(string merchantId);

    [Get("/order/v1.0/orders/{orderId}/cancellationReasons")]
    Task<IReadOnlyList<IFoodCancellationReasonResponse>> GetCancellationReasons(string orderId);

    [Post("/order/v1.0/orders/{orderId}/confirm")]
    Task ConfirmOrder(string orderId);

    [Post("/order/v1.0/orders/{orderId}/preparationStarted")]
    Task PreparationStartedOrder(string orderId);

    [Post("/order/v1.0/orders/{orderId}/readyToPickup")]
    Task ReadyToPickupOrder(string orderId);

    [Post("/order/v1.0/orders/{orderId}/dispatch")]
    Task DispatchOrder(string orderId);

    [Post("/order/v1.0/orders/{orderId}/requestCancellation")]
    Task RequestOrderCancellation(string orderId, [Body] IFoodOrderCancellationRequest request);

    [Patch("/catalog/v2.0/merchants/{merchantId}/items")]
    Task PatchProductStatus(string merchantId, [Body] IFoodPatchProductStatusRequest request);

    [Post("/order/v1.0/disputes/{disputeId}/accept")]
    Task PostHandshakeDisputesAccept(string disputeId, [Body] RespondDisputeResponse request);

    [Post("/order/v1.0/disputes/{disputeId}/reject")]
    Task PostHandshakeDisputesReject(string disputeId, [Body] RespondDisputeResponse request);

    [Post("/order/v1.0/disputes/{disputeId}/alternatives/{alternativeId}")]
    Task PostHandshakeDisputesAlternatives(string disputeId, string alternativeId, [Body] HandshakeAlternativeRequest request);
}

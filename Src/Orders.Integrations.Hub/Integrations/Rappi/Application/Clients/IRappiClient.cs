using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

using Refit;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;

public interface IRappiClient : IIntegrationClient {
    [Get("/api/webhooks")]
    Task<List<RappiWebhookEventsResponse>> GetWebhooks();

    [Put("/api/webhooks/{@event}")]
    Task<RappiWebhookEventsResponse> PutWebhookStatus([Body] RappiWebhookChangeStatusRequest request, string @event);

    [Put("/api/webhooks/{@event}/stores")]
    Task<RappiWebhookEventsResponse> PutWebhookAddNewStores([Body] List<RappiWebhookAddStoresRequest> request, string @event);

    [Delete("/api/webhooks/{@event}/stores")]
    Task<RappiWebhookRemoveStoresResponse> DeleteWebhookRemoveStores([Body] RappiWebhookRemoveStoresRequest request, string @event);

    [Put("/api/webhooks/order/new")]
    Task<RappiWebhookEventsResponse> PutWebhookChangeUrlNewOrder([Body] RappiWebhookUpdateUrlRequest request);

    [Put("/availability/stores/items")]
    Task PutAvailabilityProductsStatus([Body] List<RappiAvailabilityUpdateItemsRequest> request);

    [Post("/api/v2/restaurants-integrations-public-api/availability/items/status")]
    Task<List<RappiAvailabilityItemStatusResponse>> GetAvailabilityProducts([Body] RappiAvailabilityItemsStatusRequest request);

    [Put("/restaurants/orders/v1/stores/{storeId}/orders/{orderId}/take")]
    Task ConfirmOrder(string orderId, string storeId);

    [Put("/orders/{orderId}/take/{cookingTime}")]
    Task ConfirmeOrderWithCookingTime(string orderId, string cookingTime);

    [Put("/orders/{orderId}/reject")]
    Task RequestOrderCancellation(string orderId, [Body] RappiOrderRejectRequest request);

    [Post("/orders/{orderId}/ready-for-pickup")]
    Task ReadyToPickupOrder(string orderId);
}

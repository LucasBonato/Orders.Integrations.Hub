using BizPik.Orders.Hub.Modules.Integrations.Common.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Contracts;

public interface IRappiClient : IIntegrationClient
{
    // [Get("/api/v2/restaurants-integrations-public-api/webhook")]
    Task<List<RappiWebhookEventsResponse>> GetWebhooks();

    // [Put("/api/v2/restaurants-integrations-public-api/webhook/{event}/change-status")]
    Task<RappiWebhookEventsResponse> PutWebhookStatus(RappiWebhookChangeStatusRequest request, string @event);

    // [Put("/api/v2/restaurants-integrations-public-api/webhook/{event}/add-stores")]
    Task<RappiWebhookEventsResponse> PutWebhookAddNewStores(List<RappiWebhookAddStoresRequest> request, string @event);

    // [Delete("/api/v2/restaurants-integrations-public-api/webhook/{event}/remove-stores")]
    Task<RappiWebhookRemoveStoresResponse> DeleteWebhookRemoveStores(RappiWebhookRemoveStoresRequest request, string @event);

    // [Put("/api/v2/restaurants-integrations-public-api/webhook/NEW_ORDER/change-url")]
    Task<RappiWebhookEventsResponse> PutWebhookChangeUrlNewOrder(RappiWebhookUpdateUrlRequest request);


    // [Put("/api/v2/restaurants-integrations-public-api/availability/stores/items")]
    Task PutAvailabilityProductsStatus(List<RappiAvailabilityUpdateItemsRequest> request);

    // [Post("/api/v2/restaurants-integrations-public-api/availability/items/status")]
    Task<List<RappiAvailabilityItemStatusResponse>> GetAvailabilityProducts(RappiAvailabilityItemsStatusRequest request);


    // [Put("restaurants/orders/v1/stores/{storeId}/orders/{orderId}/take")] // CONFIRM
    Task ConfirmOrder(string orderId, string storeId);

    // [Put("/api/v2/restaurants-integrations-public-api/orders/{orderId}/take/{cookingTime]")] // CONFIRM
    // [Put("restaurants/orders/v1/stores/{storeId}/orders/{orderId}/cooking_time/{cookingTime}/take")] // CONFIRM
    Task ConfirmeOrderWithCookingTime(string orderId, string cookingTime);

    // [Put("/api/v2/restaurants-integrations-public-api/orders/{orderId}/reject")] // CANCEL
    // [Put("restaurants/orders/v1/stores/{storeId}/orders/{orderId}/cancel_type/{cancelType}/reject")] // CANCEL
    Task RequestOrderCancellation(string orderId, RappiOrderRejectRequest request);

    // [Post("/api/v2/restaurants-integrations-public-api/orders/{orderId}/ready-for-pickup")] // READY_FOR_PICKUP
    // [Post("restaurants/orders/v1/stores/{storeId}/orders/{orderId}/ready-for-pickup")] // READY_FOR_PICKUP
    Task ReadyToPickupOrder(string orderId);
}

// restaurants/orders/v1/stores/{storeId}/orders/{orderId}/bag-drink-confirmation
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

    // TODO: Check here
    // [Post("/api/v2/restaurants-integrations-public-api/availability/items/status")]
    Task<List<RappiAvailabilityItemStatusResponse>> GetAvailabilityProducts(RappiAvailabilityItemsStatusRequest request);


    // [Put("/api/v2/restaurants-integrations-public-api/orders/{orderId}/take/{cookingTime]")]
    Task PutOrderStatus(string orderId, string cookingTime);

    // [Put("/api/v2/restaurants-integrations-public-api/orders/{orderId}/reject")]
    Task PutOrderReject(RappiOrderRejectRequest request, string orderId);

    // [Post("/api/v2/restaurants-integrations-public-api/orders/{orderId}/ready-for-pickup")]
    Task PostOrderReadyForPickup(string orderId);

    // // [Put("/api/v2/restaurants-integrations-public-api/orders/{orderId}/send")]
    // Task UpdateOrderStatusToSend(string orderId);
}
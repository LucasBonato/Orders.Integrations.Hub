using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Clients;

public class RappiClient(
    HttpClient httpClient,
    ILogger<RappiClient> logger
) : IRappiClient {
    public Task<List<RappiWebhookEventsResponse>> GetWebhooks()
    {
        throw new NotImplementedException();
    }

    public Task<RappiWebhookEventsResponse> PutWebhookStatus(RappiWebhookChangeStatusRequest request, string @event)
    {
        throw new NotImplementedException();
    }

    public Task<RappiWebhookEventsResponse> PutWebhookAddNewStores(List<RappiWebhookAddStoresRequest> request, string @event)
    {
        throw new NotImplementedException();
    }

    public Task<RappiWebhookRemoveStoresResponse> DeleteWebhookRemoveStores(RappiWebhookRemoveStoresRequest request, string @event)
    {
        throw new NotImplementedException();
    }

    public Task<RappiWebhookEventsResponse> PutWebhookChangeUrlNewOrder(RappiWebhookUpdateUrlRequest request)
    {
        throw new NotImplementedException();
    }

    public Task PutAvailabilityProductsStatus(List<RappiAvailabilityUpdateItemsRequest> request)
    {
        throw new NotImplementedException();
    }

    public Task<List<RappiAvailabilityItemStatusResponse>> GetAvailabilityProducts(RappiAvailabilityItemsStatusRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task ConfirmOrder(string orderId, string storeId)
    {
        string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/take";

        HttpResponseMessage response = await httpClient.PutAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task ConfirmeOrderWithCookingTime(string orderId, string cookingTime)
    {
        string uri = $"orders/{orderId}/take/{cookingTime}";
        // string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/cooking_time/{cookingTime}/take";

        HttpResponseMessage response = await httpClient.PutAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task RequestOrderCancellation(string orderId, RappiOrderRejectRequest request)
    {
        string uri = $"orders/{orderId}/reject";
        // string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/cancel_type/{request.CancelType}/reject";

        HttpResponseMessage response = await httpClient.PutAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task ReadyToPickupOrder(string orderId)
    {
        string uri = $"orders/{orderId}/ready-for-pickup";
        // string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/ready-for-pickup";

        HttpResponseMessage response = await httpClient.PostAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }
}
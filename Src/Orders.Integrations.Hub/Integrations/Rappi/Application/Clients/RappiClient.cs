using System.Net.Http.Headers;

using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;

public class RappiClient(
    [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    IIntegrationContext integrationContext,
    HttpClient httpClient
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

    public async Task PutAvailabilityProductsStatus(List<RappiAvailabilityUpdateItemsRequest> request)
    {
        const string uri = "availability/stores/items";

        await PutGenericRequest(uri);
    }

    public async Task<List<RappiAvailabilityItemStatusResponse>> GetAvailabilityProducts(RappiAvailabilityItemsStatusRequest request)
    {
        const string uri = "/api/v2/restaurants-integrations-public-api/availability/items/status";

        HttpResponseMessage response = await PostGenericRequest(uri, request);

        return jsonSerializer.Deserialize<List<RappiAvailabilityItemStatusResponse>>(await response.Content.ReadAsStringAsync())
               ?? throw new Exception();
    }

    public async Task ConfirmOrder(string orderId, string storeId)
    {
        string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/take";

        await PutGenericRequest(uri);
    }

    public async Task ConfirmeOrderWithCookingTime(string orderId, string cookingTime)
    {
        string uri = $"orders/{orderId}/take/{cookingTime}";
        // string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/cooking_time/{cookingTime}/take";

        await PutGenericRequest(uri);
    }

    public async Task RequestOrderCancellation(string orderId, RappiOrderRejectRequest request)
    {
        string uri = $"orders/{orderId}/reject";
        // string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/cancel_type/{request.CancelType}/reject";

        await PutGenericRequest(uri);
    }

    public async Task ReadyToPickupOrder(string orderId)
    {
        string uri = $"orders/{orderId}/ready-for-pickup";
        // string uri = $"restaurants/orders/v1/stores/{storeId}/orders/{orderId}/ready-for-pickup";

        await PostGenericRequest(uri);
    }

    private async Task<HttpResponseMessage> PostGenericRequest<TRequest>(string uri, TRequest request) {
        HttpRequestMessage requestMessage = new(HttpMethod.Post, uri);

        requestMessage.SetIntegrationContext(integrationContext);

        StringContent content = new(jsonSerializer.Serialize(request));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

        return response;
    }

    private async Task PostGenericRequest(string uri) {
        HttpRequestMessage requestMessage = new(HttpMethod.Post, uri);

        requestMessage.SetIntegrationContext(integrationContext);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

    }

    private async Task PutGenericRequest(string uri) {
        HttpRequestMessage requestMessage = new(HttpMethod.Put, uri);

        requestMessage.SetIntegrationContext(integrationContext);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }
}
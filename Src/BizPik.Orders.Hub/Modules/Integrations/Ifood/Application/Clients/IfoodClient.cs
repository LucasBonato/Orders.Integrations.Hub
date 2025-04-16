using System.Text.Json;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.MerchantDetails;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Clients;

public class IfoodClient(
    HttpClient httpClient
) : IIFoodClient {
    public async Task<IfoodOrder> GetOrderDetails(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}";
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        IfoodOrder responseContent = await response.Content.ReadFromJsonAsync<IfoodOrder>()
                                        ?? throw new Exception();

        return responseContent;
    }

    public async Task<IfoodMerchant> GetMerchantDetails(string merchantId)
    {
        string uri = $"merchant/v1.0/merchants/{merchantId}";
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        IfoodMerchant responseContent = await response.Content.ReadFromJsonAsync<IfoodMerchant>()
                                        ?? throw new Exception();

        return responseContent;
    }

    public async Task ConfirmOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/confirm";

        HttpResponseMessage response = await httpClient.PostAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task PreparationStartedOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/preparationStarted";
        
        HttpResponseMessage response = await httpClient.PostAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task ReadyToPickupOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/readyToPickup";
        
        HttpResponseMessage response = await httpClient.PostAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task DispatchOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/dispatch";
        
        HttpResponseMessage response = await httpClient.PostAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task RequestOrderCancellation(string orderId, IfoodOrderCancellationRequest request)
    {
        string uri = $"order/v1.0/orders/{orderId}/requestCancellation";
        
        HttpResponseMessage response = await httpClient.PostAsync(uri, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task PatchProductStatus(string merchantId, IfoodPatchProductStatusRequest request)
    {
        string uri = $"catalog/v2.0/merchants/{merchantId}/items";
        
        HttpResponseMessage response = await httpClient.PatchAsync(uri, new StringContent(JsonSerializer.Serialize(request)));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }
}
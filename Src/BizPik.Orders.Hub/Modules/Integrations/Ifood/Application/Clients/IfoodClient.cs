using System.Net;
using System.Text.Json;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.MerchantDetails;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Clients;

public class IfoodClient(
    ILogger<IfoodClient> logger,
    HttpClient httpClient
) : IIFoodClient {
    public async Task<IfoodOrder> GetOrderDetails(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}";
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(await response.Content.ReadAsStringAsync());
        }

        logger.LogInformation("[INFO] - IfoodClient - OrderDetails: {body}", await response.Content.ReadAsStringAsync());

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
        
        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(JsonSerializer.Serialize(request)));
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

    public async Task<IReadOnlyList<IfoodCancellationReasonResponse>> GetCancellationReasons(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/cancellationReasons";

        HttpResponseMessage response = await httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

        if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0)
        {
            return [];
        }

        IReadOnlyList<IfoodCancellationReasonResponse> responseContent = await response.Content.ReadFromJsonAsync<IReadOnlyList<IfoodCancellationReasonResponse>>()?? [];

        return responseContent;
    }
}
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Clients;

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

        HttpRequestMessage requestMessage = new (HttpMethod.Post, uri);
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.Default,"application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception(responseContent);
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

    public async Task PostHandshakeDisputesAccept(string disputeId, RespondDisputeResponse request)
    {
        string uri = $"order/v1.0/disputes/{disputeId}/accept";

        HttpRequestMessage requestMessage = new (HttpMethod.Post, uri);
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.Default,"application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task PostHandshakeDisputesReject(string disputeId, RespondDisputeResponse request)
    {
        string uri = $"order/v1.0/disputes/{disputeId}/reject";

        HttpRequestMessage requestMessage = new (HttpMethod.Post, uri);
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.Default,"application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task PostHandshakeDisputesAlternatives(string disputeId, string alternativeId, HandshakeAlternativeRequest request)
    {
        string uri = $"order/v1.0/disputes/{disputeId}/alternatives/{alternativeId}";

        HttpRequestMessage requestMessage = new (HttpMethod.Post, uri);
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.Default,"application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception(responseContent);
        }
    }
}
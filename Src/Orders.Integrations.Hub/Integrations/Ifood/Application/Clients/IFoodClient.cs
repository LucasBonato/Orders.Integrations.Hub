using System.Net;
using System.Net.Http.Headers;
using System.Text;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Clients;

public class IFoodClient(
    [FromKeyedServices(OrderIntegration.IFOOD)] ICustomJsonSerializer jsonSerializer,
    ILogger<IFoodClient> logger,
    HttpClient httpClient
) : IIFoodClient {
    public async Task<DownloadFile> GetDisputeImage(string uri)
    {
        HttpResponseMessage response = await httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(await response.Content.ReadAsStringAsync());
        }

        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

        return new DownloadFile(
            Bytes: fileBytes,
            ContentType: response.Content.Headers.ContentType?.MediaType?? "application/octet-stream"
        );
    }

    public async Task<IfoodOrder> GetOrderDetails(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}";
        HttpResponseMessage response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(await response.Content.ReadAsStringAsync());
        }

        logger.LogInformation("[INFO] - IFoodClient - OrderDetails: {body}", await response.Content.ReadAsStringAsync());

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
        var content = new StringContent(jsonSerializer.Serialize(request), Encoding.Default,"application/json");
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
        
        HttpResponseMessage response = await httpClient.PatchAsync(uri, new StringContent(jsonSerializer.Serialize(request)));
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
        // if (!string.IsNullOrEmpty(request.Reason))
        //     return;

        string uri = $"order/v1.0/disputes/{disputeId}/accept";

        HttpRequestMessage requestMessage = new (HttpMethod.Post, uri);
        var content = new StringContent(jsonSerializer.Serialize(request), Encoding.Default,"application/json");
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
        var content = new StringContent(jsonSerializer.Serialize(request), Encoding.Default,"application/json");
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
        // if (request.Type is HandshakeAlternativeType.ADDITIONAL_TIME)
        //     return;

        string uri = $"order/v1.0/disputes/{disputeId}/alternatives/{alternativeId}";

        HttpRequestMessage requestMessage = new (HttpMethod.Post, uri);
        var content = new StringContent(jsonSerializer.Serialize(request), Encoding.Default,"application/json");
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
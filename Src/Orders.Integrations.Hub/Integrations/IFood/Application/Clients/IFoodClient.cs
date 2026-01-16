using System.Net.Http.Headers;
using System.Text;

using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.MerchantDetails;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Clients;

public class IFoodClient(
    [FromKeyedServices(IFoodIntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    IIntegrationContext integrationContext,
    ILogger<IFoodClient> logger,
    HttpClient httpClient
) : IIFoodClient {
    public async Task<DownloadFile> GetDisputeImage(string uri)
    {
        HttpRequestMessage request = new(HttpMethod.Get, uri);

        request.SetIntegrationContext(integrationContext);

        HttpResponseMessage response = await httpClient.SendAsync(request);

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

    public async Task<IFoodOrder> GetOrderDetails(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}";
        IFoodOrder order = await GetGenericRequest<IFoodOrder>(uri);
        logger.LogInformation("[INFO] - IfoodClient - OrderDetails: {body}", jsonSerializer.Serialize(order));
        return order;
    }

    public async Task<IFoodMerchant> GetMerchantDetails(string merchantId)
    {
        string uri = $"merchant/v1.0/merchants/{merchantId}";
        IFoodMerchant merchant = await GetGenericRequest<IFoodMerchant>(uri);
        return merchant;
    }

    public async Task ConfirmOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/confirm";

        await PostGenericRequest(uri);
    }

    public async Task PreparationStartedOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/preparationStarted";

        await PostGenericRequest(uri);
    }

    public async Task ReadyToPickupOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/readyToPickup";

        await PostGenericRequest(uri);
    }

    public async Task DispatchOrder(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/dispatch";

        await PostGenericRequest(uri);
    }

    public async Task RequestOrderCancellation(string orderId, IFoodOrderCancellationRequest request)
    {
        string uri = $"order/v1.0/orders/{orderId}/requestCancellation";

        await PostGenericRequest(uri, request);
    }

    public async Task PatchProductStatus(string merchantId, IFoodPatchProductStatusRequest request)
    {
        string uri = $"catalog/v2.0/merchants/{merchantId}/items";

        HttpRequestMessage requestMessage = new(HttpMethod.Patch, uri);
        requestMessage.SetIntegrationContext(integrationContext);
        StringContent content = new(jsonSerializer.Serialize(request));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }

    public async Task<IReadOnlyList<IFoodCancellationReasonResponse>> GetCancellationReasons(string orderId)
    {
        string uri = $"order/v1.0/orders/{orderId}/cancellationReasons";
        IReadOnlyList<IFoodCancellationReasonResponse> response = await GetGenericRequest<IReadOnlyList<IFoodCancellationReasonResponse>>(uri);

        return response;
    }

    public async Task PostHandshakeDisputesAccept(string disputeId, RespondDisputeResponse request)
    {
        string uri = $"order/v1.0/disputes/{disputeId}/accept";

        await PostGenericRequest(uri, request);
    }

    public async Task PostHandshakeDisputesReject(string disputeId, RespondDisputeResponse request)
    {
        string uri = $"order/v1.0/disputes/{disputeId}/reject";

        await PostGenericRequest(uri, request);
    }

    public async Task PostHandshakeDisputesAlternatives(string disputeId, string alternativeId, HandshakeAlternativeRequest request)
    {
        string uri = $"order/v1.0/disputes/{disputeId}/alternatives/{alternativeId}";

        await PostGenericRequest(uri, request);
    }

    private async Task<TResponse> GetGenericRequest<TResponse>(string uri)
    {
        HttpRequestMessage request = new(HttpMethod.Get, uri);

        request.SetIntegrationContext(integrationContext);

        HttpResponseMessage response = await httpClient.SendAsync(request);

        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }

        return jsonSerializer.Deserialize<TResponse>(responseContent)?? throw new Exception();
    }

    private async Task PostGenericRequest<T>(string uri, T? body)
    {
        logger.LogInformation("[INFO] - ChangeStatus - [{uri}]: {payload}", uri, jsonSerializer.Serialize(body));

        HttpRequestMessage request = new (HttpMethod.Post, uri);
        request.SetIntegrationContext(integrationContext);
        var content = new StringContent(jsonSerializer.Serialize(request), Encoding.Default,"application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception(responseContent);
        }
    }

    private async Task PostGenericRequest(string uri)
    {
        HttpRequestMessage request = new (HttpMethod.Post, uri);
        request.SetIntegrationContext(integrationContext);

        HttpResponseMessage response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception(responseContent);
        }
    }
}
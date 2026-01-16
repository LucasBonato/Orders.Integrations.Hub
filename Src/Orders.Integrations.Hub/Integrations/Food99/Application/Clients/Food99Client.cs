using System.Net.Http.Headers;
using System.Text;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Clients;

public class Food99Client(
    [FromKeyedServices(Food99IntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    IIntegrationContext integrationContext,
    ILogger<Food99Client> logger,
    HttpClient httpClient
) : IFood99Client {
    public async Task ConfirmOrder(string appShopId, string orderId)
    {
        const string uri = "/v1/order/order/confirm";

        HttpResponseMessage response = await PostGenericRequest(uri, new Food99StatusChangeRequest(orderId,null, null, null));
        
        logger.LogInformation("[INFO] ConfirmOrder-Result: {result}", response.Content.ReadAsStringAsync().Result);
    }

    public async Task ReadyToPickupOrder(string appShopId, string orderId)
    {
        const string uri = "/v1/order/order/ready";

        HttpResponseMessage response = await PostGenericRequest(uri, new Food99StatusChangeRequest(orderId, null, null, null));

        logger.LogInformation("[INFO] ReadyToPickupOrder-Result: {result}", response.Content.ReadAsStringAsync().Result);
    }

    public async Task DeliveredOrder(string appShopId, string orderId)
    {
        const string uri = "/v1/order/order/delivered";

        HttpResponseMessage response = await PostGenericRequest(uri, new Food99StatusChangeRequest(orderId, null, null, null));

        logger.LogInformation("[INFO] DeliveredOrder-Result: {result}", response.Content.ReadAsStringAsync().Result);
    }

    public async Task CancelOrder(string appShopId, string orderId, string reason, int reasonId)
    {
        const string uri = "/v1/order/order/cancel";

        HttpResponseMessage response = await PostGenericRequest(uri, new Food99StatusChangeRequest(
            OrderId: orderId,
            AuthToken: null, // The AuthToken will be added by the AuthHandler
            ReasonId: reasonId,
            Reason: reason
        ));

        logger.LogInformation("[INFO] CancelOrder-Result: {result}", response.Content.ReadAsStringAsync().Result);        
    }

    private async Task<HttpResponseMessage> PostGenericRequest<T>(string uri, T request)
    {
        logger.LogInformation("[INFO] - ChangeStatus - [{uri}]: {payload}", uri, jsonSerializer.Serialize(request));

        HttpRequestMessage requestMessage = new (HttpMethod.Post, uri);
        requestMessage.SetIntegrationContext(integrationContext);
        StringContent content = new(jsonSerializer.Serialize(request), Encoding.UTF8,"application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception(responseContent);
        }

        return response;
    }
}
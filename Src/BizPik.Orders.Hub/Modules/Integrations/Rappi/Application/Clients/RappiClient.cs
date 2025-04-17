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

    public Task PutOrderStatus(string orderId, string cookingTime)
    {
        throw new NotImplementedException();
    }

    public Task PutOrderReject(RappiOrderRejectRequest request, string orderId)
    {
        throw new NotImplementedException();
    }

    public Task PostOrderReadyForPickup(string orderId)
    {
        throw new NotImplementedException();
    }
}
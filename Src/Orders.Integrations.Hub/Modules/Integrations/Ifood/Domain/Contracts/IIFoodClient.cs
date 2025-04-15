using Orders.Integrations.Hub.Modules.Integrations.Common.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;

public interface IIFoodClient : IIntegrationClient
{
    Task<IfoodOrder> GetOrderDetails(string orderId);
    Task ConfirmOrder(string orderId);
    Task PreparationStartedOrder(string orderId);
    Task ReadyToPickupOrder(string orderId);
    Task DispatchOrder(string orderId);
    Task RequestOrderCancellation(string orderId, IfoodOrderCancellationRequest request);
    Task PatchProductStatus(string merchantId, IfoodPatchProductStatusRequest request);
}
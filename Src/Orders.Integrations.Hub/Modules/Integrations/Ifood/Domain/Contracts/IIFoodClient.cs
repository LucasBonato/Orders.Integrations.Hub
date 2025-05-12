using Orders.Integrations.Hub.Modules.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.MerchantDetails;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;

public interface IIFoodClient : IIntegrationClient
{
    Task<IfoodOrder> GetOrderDetails(string orderId);
    Task<IfoodMerchant> GetMerchantDetails(string merchantId);
    Task ConfirmOrder(string orderId);
    Task PreparationStartedOrder(string orderId);
    Task ReadyToPickupOrder(string orderId);
    Task DispatchOrder(string orderId);
    Task RequestOrderCancellation(string orderId, IfoodOrderCancellationRequest request);
    Task PatchProductStatus(string merchantId, IfoodPatchProductStatusRequest request);

    Task<IReadOnlyList<IfoodCancellationReasonResponse>> GetCancellationReasons(string orderId);
}
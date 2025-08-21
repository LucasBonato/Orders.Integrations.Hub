using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.MerchantDetails;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;

public interface IIFoodClient : IIntegrationClient
{
    Task<DownloadFile> GetDisputeImage(string uri);
    Task<IFoodOrder> GetOrderDetails(string orderId);
    Task<IFoodMerchant> GetMerchantDetails(string merchantId);
    Task<IReadOnlyList<IFoodCancellationReasonResponse>> GetCancellationReasons(string orderId);
    Task ConfirmOrder(string orderId);
    Task PreparationStartedOrder(string orderId);
    Task ReadyToPickupOrder(string orderId);
    Task DispatchOrder(string orderId);
    Task RequestOrderCancellation(string orderId, IFoodOrderCancellationRequest request);
    Task PatchProductStatus(string merchantId, IFoodPatchProductStatusRequest request);

    Task PostHandshakeDisputesAccept(string disputeId, RespondDisputeResponse request);
    Task PostHandshakeDisputesReject(string disputeId, RespondDisputeResponse request);
    Task PostHandshakeDisputesAlternatives(string disputeId, string alternativeId, HandshakeAlternativeRequest request);
}
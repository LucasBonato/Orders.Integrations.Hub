using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Common.Contracts;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Contracts;

public interface IIFoodClient : IIntegrationClient
{
    Task<IfoodOrder> GetOrderDetails(string orderId);
    Task<IfoodMerchant> GetMerchantDetails(string merchantId);
    Task<IReadOnlyList<IfoodCancellationReasonResponse>> GetCancellationReasons(string orderId);
    Task ConfirmOrder(string orderId);
    Task PreparationStartedOrder(string orderId);
    Task ReadyToPickupOrder(string orderId);
    Task DispatchOrder(string orderId);
    Task RequestOrderCancellation(string orderId, IfoodOrderCancellationRequest request);
    Task PatchProductStatus(string merchantId, IfoodPatchProductStatusRequest request);

    Task PostHandshakeDisputesAccept(string disputeId, RespondDisputeResponse request);
    Task PostHandshakeDisputesReject(string disputeId, RespondDisputeResponse request);
    Task PostHandshakeDisputesAlternatives(string disputeId, string alternativeId, HandshakeAlternativeRequest request);
}
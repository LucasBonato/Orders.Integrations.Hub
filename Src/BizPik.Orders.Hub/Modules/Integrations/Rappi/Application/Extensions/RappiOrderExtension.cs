using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Extensions;

public static class RappiOrderExtension
{
    public static Order ToOrder(this RappiOrder order, int companyId)
    {
        throw new NotImplementedException();
        // return new Order(
        //     OrderId: order.OrderDetail.OrderId,
        //     Type: order.OrderDetail.DeliveryMethod.ToOrder(),
        //     DisplayId: Guid.NewGuid().ToString()[..5],
        //     SourceAppId: OrderSalesChannel.RAPPI.ToString(),
        //     SalesChannel: OrderSalesChannel.RAPPI.ToString(),
        //     VirtualBrand: order.Store.InternalId,
        //     CreatedAt: order.OrderDetail.CreatedAt,
        //     LastEvent: OrderEventType.CREATED,
        //     OrderTiming: order.OrderDetail..OrderTiming.ToOrder(),
        //     PreparationStartDateTime: order.PreparationStartDateTime,
        //     Merchant: new OrderMerchant(
        //         Id: order.Merchant.Id,
        //         Name: order.Merchant.Name
        //     ),
        //     Items: items,
        //     OtherFees: otherFees,
        //     Discounts: discounts,
        //     Total: new OrderTotal(
        //         Discount: order.Total.Benefits.ToBrl(),
        //         ItemsPrice: order.Total.SubTotal.ToBrl(),
        //         OrderAmount: order.Total.OrderAmount.ToBrl(),
        //         OtherFees: (order.Total.DeliveryFee + order.Total.AdditionalFees).ToBrl()
        //     ),
        //     Payments: new OrderPayment(
        //         Prepaid: (int)order.Payments.Prepaid,
        //         Pending: (double)order.Payments.Pending,
        //         Methods: paymentMethods
        //     ),
        //     TaxInvoice: null,
        //     Customer: new OrderCustomer(
        //         Id: order.Customer.Id,
        //         Name: order.Customer.Name,
        //         DocumentNumber: order.Customer.DocumentNumber,
        //         Email: string.Empty,
        //         OrdersCountOnMerchant: order.Customer.OrdersCountOnMerchant,
        //         Phone: new Phone(
        //             Extension: order.Customer.Phone?.Localizer?? string.Empty,
        //             Number: order.Customer.Phone?.Number?? string.Empty
        //         )
        //     ),
        //     Schedule: orderSchedule,
        //     OrderPriority: order.Delivery?.Mode.ToOrderPriority(),
        //     Delivery: orderDelivery,
        //     Takeout: orderTakeout,
        //     Indoor: orderIndoor,
        //     SendPreparing: false,
        //     SendDelivered: false,
        //     SendPickedUp: false,
        //     SendTracking: false,
        //     ExtraInfo: order.ExtraInfo?? string.Empty,
        //     CompanyId: companyId,
        //     OrderDisplayId: Guid.NewGuid().ToString()[..5],
        //     ExternalId: order.Id
        // );
    }

    private static OrderEventType ToOrderEvent(this RappiWebhookOrderEvent orderEvent) {
        return orderEvent switch {
            RappiWebhookOrderEvent.taken_visible_order => OrderEventType.CONFIRMED,
            RappiWebhookOrderEvent.replace_storekeeper => OrderEventType.DELIVERYMAN_CHANGED,
            RappiWebhookOrderEvent.ready_for_pick_up => OrderEventType.READY_FOR_PICKUP,
            RappiWebhookOrderEvent.domiciliary_in_store => OrderEventType.DELIVERYMAN_IN_STORE,
            RappiWebhookOrderEvent.hand_to_domiciliary => OrderEventType.DISPATCHED,
            RappiWebhookOrderEvent.arrive => OrderEventType.DELIVERED,
            RappiWebhookOrderEvent.close_order => OrderEventType.CONCLUDED,
            RappiWebhookOrderEvent.canceled_by_fraud_automation
                or RappiWebhookOrderEvent.canceled_from_cms
                or RappiWebhookOrderEvent.canceled_store_closed
                or RappiWebhookOrderEvent.canceled_with_charge
                or RappiWebhookOrderEvent.cancel_by_application_user
                or RappiWebhookOrderEvent.cancel_by_user
                or RappiWebhookOrderEvent.cancel_by_sk_with_charge
                or RappiWebhookOrderEvent.cancel_by_support
                or RappiWebhookOrderEvent.cancel_by_support_with_charge
                or RappiWebhookOrderEvent.cancel_without_charges => OrderEventType.CANCELLED,
            _ => OrderEventType.CONFIRMED
        };
    }

    public static OrderUpdateStatus FromRappi(this RappiWebhookEventOrderRequest request)
    {
        return new OrderUpdateStatus(
            OrderId: request.OrderId,
            SourceAppId: "rappi",
            Type: request.OrderEvent.ToOrderEvent(),
            CreateAt: request.EventTime?? DateTime.UtcNow,
            FromIntegration: true
        );
    }

    public static OrderUpdateStatus FromRappi(this RappiOrder request, OrderEventType eventType)
    {
        return new OrderUpdateStatus(
            OrderId: request.OrderDetail.OrderId,
            SourceAppId: "rappi",
            Type: eventType,
            CreateAt: request.OrderDetail.CreatedAt,
            FromIntegration: false
        );
    }
}
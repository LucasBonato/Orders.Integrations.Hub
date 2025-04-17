using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Merchant;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Extensions;

public static class RappiOrderExtension
{
    public static Order ToOrder(this RappiOrder order, int companyId)
    {
        return new Order(
            OrderId: order.OrderDetail.OrderId,
            Type: order.OrderDetail.DeliveryMethod.ToOrder(),
            DisplayId: Guid.NewGuid().ToString()[..5],
            SourceAppId: OrderSalesChannel.RAPPI.ToString(),
            SalesChannel: OrderSalesChannel.RAPPI.ToString(),
            VirtualBrand: order.Store.InternalId,
            CreatedAt: order.OrderDetail.CreatedAt,
            LastEvent: OrderEventType.CREATED,
            OrderTiming: order.OrderDetail..OrderTiming.ToOrder(),
            PreparationStartDateTime: order.PreparationStartDateTime,
            Merchant: new OrderMerchant(
                Id: order.Merchant.Id,
                Name: order.Merchant.Name
            ),
            Items: items,
            OtherFees: otherFees,
            Discounts: discounts,
            Total: new OrderTotal(
                Discount: order.Total.Benefits.ToBrl(),
                ItemsPrice: order.Total.SubTotal.ToBrl(),
                OrderAmount: order.Total.OrderAmount.ToBrl(),
                OtherFees: (order.Total.DeliveryFee + order.Total.AdditionalFees).ToBrl()
            ),
            Payments: new OrderPayment(
                Prepaid: (int)order.Payments.Prepaid,
                Pending: (double)order.Payments.Pending,
                Methods: paymentMethods
            ),
            TaxInvoice: null,
            Customer: new OrderCustomer(
                Id: order.Customer.Id,
                Name: order.Customer.Name,
                DocumentNumber: order.Customer.DocumentNumber,
                Email: string.Empty,
                OrdersCountOnMerchant: order.Customer.OrdersCountOnMerchant,
                Phone: new Phone(
                    Extension: order.Customer.Phone?.Localizer?? string.Empty,
                    Number: order.Customer.Phone?.Number?? string.Empty
                )
            ),
            Schedule: orderSchedule,
            OrderPriority: order.Delivery?.Mode.ToOrderPriority(),
            Delivery: orderDelivery,
            Takeout: orderTakeout,
            Indoor: orderIndoor,
            SendPreparing: false,
            SendDelivered: false,
            SendPickedUp: false,
            SendTracking: false,
            ExtraInfo: order.ExtraInfo?? string.Empty,
            CompanyId: companyId,
            OrderDisplayId: Guid.NewGuid().ToString()[..5],
            ExternalId: order.Id
        );
    }
}
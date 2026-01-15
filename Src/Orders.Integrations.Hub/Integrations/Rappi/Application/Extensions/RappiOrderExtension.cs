using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Entity.Address;
using Orders.Integrations.Hub.Core.Domain.Entity.Discount;
using Orders.Integrations.Hub.Core.Domain.Entity.Item;
using Orders.Integrations.Hub.Core.Domain.Entity.Merchant;
using Orders.Integrations.Hub.Core.Domain.Entity.Payment;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;

public static class RappiOrderExtension
{
    public static Order ToOrder(this RappiOrder order, string tenantId)
    {

        decimal? orderTotal = order.OrderDetail.Totals.TotalProducts
                              + order.OrderDetail.Totals.Charges.Shipping
                              + order.OrderDetail.Totals.Charges.ServiceFee
                              - order.OrderDetail.Totals.TotalDiscounts
            // - (orderRappi.OrderDetail.Discounts ?? []).Sum(discount => discount.AmountByRappi)
            ;
        List<OrderItem> items = order.OrderDetail.Items.Select(item => new OrderItem(
            Id: item.Id,
            Index: 0,
            Name: item.Name,
            ExternalCode: item.Sku ?? string.Empty,
            ImageUrl: string.Empty,
            Unit: OrderUnit.UN,
            Ean: string.Empty,
            Quantity: item.Quantity,
            SpecialInstructions: item.Comments?? string.Empty,
            UnitPrice: (item.Price / item.Quantity).ToBrl(),
            TotalPrice: item.Price.ToBrl(),
            OptionsPrice: item.Subitems.Sum(option => option.Price).ToBrl(),
            Options: item.Subitems.Select(option  => new OrderItemOption(
                Id: option.Id,
                Index: 0,
                Name: option.Name,
                ExternalCode: option.Sku ?? string.Empty,
                ImageUrl: string.Empty,
                Unit: OrderUnit.UN,
                Ean: string.Empty,
                Quantity: option.Quantity?? 0,
                SpecialInstructions: string.Empty,
                UnitPrice: (option.Price / option.Quantity).ToBrl(),
                TotalPrice: option.Price.ToBrl()
            )).ToList()
        )).ToList();

        List<OrderDiscount> discounts = order.OrderDetail.Discounts?.Select(discount => new OrderDiscount(
            Amount: discount.Value.ToBrl(),
            Target: discount.Type.ToOrder(),
            TargetId: discount.ProductId.ToString()?? string.Empty,
            SponsorshipValues: [
                new OrderDiscountSponsorshipValue(
                    Amount: discount.AmountByRappi.ToBrl(),
                    Name: OrderSponsorshipName.MARKETPLACE,
                    Description: discount.Description
                ),
                new OrderDiscountSponsorshipValue(
                    Amount: discount.AmountByPartner.ToBrl(),
                    Name: OrderSponsorshipName.MERCHANT,
                    Description: discount.Description
                )
            ]
        )).ToList() ?? [];

        List<OrderPaymentMethod> paymentMethods = [
            new OrderPaymentMethod(
                Value: orderTotal?? 0,
                Currency: "BRL",
                Type: order.OrderDetail.BillingInformation == null ? MethodType.PREPAID : MethodType.PENDING,
                Method: order.OrderDetail.PaymentMethod.ToOrder(),
                Brand: MethodBrand.OTHER,
                MethodInfo: string.Empty,
                ChangeFor: null,
                Transaction: null
            )
        ];

        OrderDelivery? orderDelivery = order.OrderDetail.DeliveryInformation is { } delivery
            ? new OrderDelivery(
                PickupCode: null,
                DeliveredBy: OrderDeliveredBy.MERCHANT,
                DeliveryDateTime: DateTime.UtcNow.AddMinutes(order.OrderDetail.CookingTime ?? 0),
                EstimatedDeliveryDateTime: DateTime.Now,
                DeliveryAddress: new Address(
                    City: delivery.City,
                    Complement: delivery.Complement,
                    Country: "BR",
                    District: delivery.Neighborhood,
                    FormattedAddress: delivery.CompleteAddress,
                    Number: delivery.StreetNumber,
                    PostalCode: delivery.PostalCode,
                    Reference: string.Empty,
                    State: "São Paulo",
                    Street: delivery.StreetName,
                    Coordinates: new AddressCoordinates(
                        Longitude: 0,
                        Latitude: 0
                    )
                )
            )
            : null;

        return new Order(
            OrderId: order.OrderDetail.OrderId,
            Type: OrderType.DELIVERY,
            DisplayId: Guid.NewGuid().ToString()[..5],
            SourceAppId: nameof(OrderSalesChannel.RAPPI),
            SalesChannel: nameof(OrderSalesChannel.RAPPI),
            VirtualBrand: order.Store.InternalId,
            CreatedAt: order.OrderDetail.CreatedAt,
            LastEvent: OrderEventType.CREATED,
            OrderTiming: OrderTiming.INSTANT,
            PreparationStartDateTime: DateTime.Now,
            Merchant: new OrderMerchant(
                Id: order.Store.ExternalId,
                Name: order.Store.Name
            ),
            Items: items,
            OtherFees: order.MapRappiOtherFeesToOpenDelivery(),
            Discounts: discounts,
            Total: new OrderTotal(
                Discount: order.OrderDetail.Totals.TotalDiscounts.ToBrl(),
                ItemsPrice: order.OrderDetail.Totals.TotalProducts.ToBrl(),
                OrderAmount: order.OrderDetail.Totals.TotalOrder.ToBrl(),
                OtherFees: (order.OrderDetail.Totals.Charges.ServiceFee + order.OrderDetail.Totals.Charges.Shipping).ToBrl()
            ),
            Payments: new OrderPayment(
                Prepaid: 0,
                Pending: order.OrderDetail.Totals.TotalToPay?? 0,
                Methods: paymentMethods
            ),
            TaxInvoice: null,
            Customer: new OrderCustomer(
                Id: Guid.NewGuid().ToString()[5..],
                Name: order.Customer?.FirstName + " " + order.Customer?.LastName,
                DocumentNumber: order.Customer?.DocumentNumber?? string.Empty,
                Email: order.OrderDetail.BillingInformation?.Email,
                OrdersCountOnMerchant: 0,
                Phone: new Phone(
                    Extension: string.Empty,
                    Number: order.Customer?.PhoneNumber?? string.Empty
                )
            ),
            Schedule: null,
            OrderPriority: OrderPriority.PRIORITY5,
            Delivery: orderDelivery,
            Takeout: null,
            Indoor: null,
            SendPreparing: false,
            SendDelivered: false,
            SendPickedUp: false,
            SendTracking: false,
            ExtraInfo: string.Empty,
            TenantId: tenantId,
            OrderDisplayId: Guid.NewGuid().ToString()[..5],
            ExternalId: order.OrderDetail.OrderId,
            Dispute: null
        );
    }

    private static MethodMethod ToOrder(this RappiOrderPaymentMethod method)
    {
        return method switch
        {
            RappiOrderPaymentMethod.visa_checkout => MethodMethod.CREDIT,
            RappiOrderPaymentMethod.rappi_pay or
                RappiOrderPaymentMethod.paypal or
                RappiOrderPaymentMethod.webpay or
                RappiOrderPaymentMethod.google_pay or
                RappiOrderPaymentMethod.apple_pay or
                RappiOrderPaymentMethod.masterpass or
                RappiOrderPaymentMethod.dc => MethodMethod.DIGITAL_WALLET,
            RappiOrderPaymentMethod.pos_terminal or
                RappiOrderPaymentMethod.elo => MethodMethod.DEBIT,
            RappiOrderPaymentMethod.vale_r or
                RappiOrderPaymentMethod.ticket_r or
                RappiOrderPaymentMethod.sodexo => MethodMethod.MEAL_VOUCHER,
            RappiOrderPaymentMethod.PIX => MethodMethod.PIX,
            RappiOrderPaymentMethod.cash => MethodMethod.CASH,
            RappiOrderPaymentMethod.rappicorp => MethodMethod.CREDIT_DEBIT,
            RappiOrderPaymentMethod.cc => MethodMethod.COUPON,
            RappiOrderPaymentMethod.edenred => MethodMethod.REDEEM,
            _ => MethodMethod.OTHER
        };
    }

    private static DiscountTarget ToOrder(this string _)
    {
        return DiscountTarget.ITEM;
    }

    private static List<OrderFee> MapRappiOtherFeesToOpenDelivery(this RappiOrder order)
    {
        List<OrderFee> otherFees = [];

        if (order.OrderDetail.Totals.Charges.Shipping is not null)
        {
            otherFees.Add(new OrderFee(
                Name: "TIP",
                Type: FeeType.DELIVERY_FEE,
                ReceivedBy: FeeReceivedBy.MERCHANT,
                Price: order.OrderDetail.Totals.Charges.Shipping.ToBrl(),
                ReceiverDocument: "None",
                Observation: "The delivery fee"
            ));
        }

        if (order.OrderDetail.Totals.Charges.ServiceFee is not null)
        {
            otherFees.Add(new OrderFee(
                Name: "TIP",
                Type: FeeType.SERVICE_FEE,
                ReceivedBy: FeeReceivedBy.MERCHANT,
                Price: order.OrderDetail.Totals.Charges.ServiceFee.ToBrl(),
                ReceiverDocument: "None",
                Observation: "The service fee"
            ));
        }

        if (order.OrderDetail.Totals.OtherTotals.Tip is not null)
        {
            otherFees.Add(new OrderFee(
                Name: "TIP",
                Type: FeeType.TIP,
                ReceivedBy: FeeReceivedBy.MERCHANT,
                Price: order.OrderDetail.Totals.OtherTotals.Tip.ToBrl(),
                ReceiverDocument: "None",
                Observation: "The delivery fee"
            ));
        }

        return otherFees;
    }

    private static Price ToBrl(this decimal? price)
    {
        return new Price(
            Value: price?? 0,
            Currency: "BRL"
        );
    }

    private static Price ToBrl(this decimal price)
    {
        return new Price(
            Value: price,
            Currency: "BRL"
        );
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

    public static OrderUpdate FromRappi(this RappiWebhookEventOrderRequest request)
    {
        return new OrderUpdate(
            OrderId: request.OrderId,
            SourceAppId: RappiIntegrationKey.RAPPI,
            Type: request.OrderEvent.ToOrderEvent(),
            CreateAt: request.EventTime?? DateTime.UtcNow,
            Dispute: null,
            FromIntegration: true
        );
    }

    public static OrderUpdate FromRappi(this RappiOrder request, OrderEventType eventType)
    {
        return new OrderUpdate(
            OrderId: request.OrderDetail.OrderId,
            SourceAppId: RappiIntegrationKey.RAPPI,
            Type: eventType,
            CreateAt: request.OrderDetail.CreatedAt,
            Dispute: null,
            FromIntegration: false
        );
    }
}
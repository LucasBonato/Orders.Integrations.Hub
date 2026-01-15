using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Entity.Address;
using Orders.Integrations.Hub.Core.Domain.Entity.Discount;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Entity.Item;
using Orders.Integrations.Hub.Core.Domain.Entity.Merchant;
using Orders.Integrations.Hub.Core.Domain.Entity.Payment;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Extensions;

public static class Food99OrderExtension
{
    public static Order ToOrder(this Food99WebhookRequest order, string tenantId)
    {
        Food99OrderInfo orderInfo = order.Data.OrderInfo;

        List<OrderItem> items = orderInfo.OrderItems.Select((item, index) => new OrderItem(
            Id: item.AppItemId,
            Index: index,
            Name: item.Name,
            ExternalCode: string.IsNullOrEmpty(item.AppExternalId) ? item.AppItemId : item.AppExternalId,
            ImageUrl: string.Empty,
            Unit: OrderUnit.UN,
            Ean: string.Empty,
            Quantity: item.Amount,
            SpecialInstructions: item.Remark,
            UnitPrice: item.SkuPrice.ToBrl(),
            TotalPrice: item.TotalPrice.ToBrl(),
            OptionsPrice: item.SubItemList.Sum(option => option.SkuPrice).ToBrl(),
            Options: item.SubItemList.Select((option, optionIndex)  => new OrderItemOption(
                Id: option.AppItemId,
                Index: optionIndex,
                Name: option.Name,
                ExternalCode: string.IsNullOrEmpty(option.AppExternalId) ? option.AppItemId : option.AppExternalId,
                ImageUrl: string.Empty,
                Unit: OrderUnit.UN,
                Ean: string.Empty,
                Quantity: option.Amount,
                SpecialInstructions: string.Empty,
                UnitPrice: option.SkuPrice.ToBrl(),
                TotalPrice: option.TotalPrice.ToBrl()
            )).ToList()
        )).ToList();

        Dictionary<Food99OrderPromoType, int> itemPromoCounts = orderInfo.OrderItems
            .SelectMany(item => item.PromoList ?? Enumerable.Empty<Food99Promotion>())
            .GroupBy(promotion => promotion.PromoType)
            .ToDictionary(group => group.Key, group => group.Count());

        List<OrderDiscount> discounts = orderInfo.OrderItems
            .Where(item => item.PromotionDetail != null)
            .Where(item => item.PromoList != null)
            .Select(item => {
                return new OrderDiscount(
                    Amount: item.PromotionDetail!.PromoDiscount.ToBrl(),
                    Target: item.PromotionDetail.PromoType.ToOrder(isItemLevel: true),
                    TargetId: item.AppItemId,
                    SponsorshipValues: item.PromoList!
                        .SelectMany(promo => promo.ToSponsorshipValues())
                        .ToList()
                );
            })
            .ToList();

        if (orderInfo.Promotions != null)
        {
            foreach (Food99Promotion promo in orderInfo.Promotions)
            {
                // Check if this promoType exists in the item-level dictionary
                if (itemPromoCounts.TryGetValue(promo.PromoType, out var count) && count > 0)
                {
                    // This one is already accounted at the item-level: decrement the count and skip
                    itemPromoCounts[promo.PromoType]--;
                    continue;
                }

                discounts.Add(
                    new OrderDiscount(
                        Amount: promo.PromoDiscount.ToBrl(),
                        Target: promo.PromoType.ToOrder(),
                        TargetId: string.Empty,
                        SponsorshipValues: promo.ToSponsorshipValues()
                    )
                );
            }
        }

        List<OrderPaymentMethod> paymentMethods = [
            new(
                Value: (orderInfo.Price.RealPayPrice ?? 0).ToDecimalValue(),
                Currency: "BRL",
                Type: orderInfo.PayType is Food99OrderPaymentMethod.OnlinePayment ? MethodType.PREPAID : MethodType.PENDING,
                Method: orderInfo.PayType.ToOrder(),
                Brand: MethodBrand.OTHER,
                MethodInfo: string.Empty,
                ChangeFor: orderInfo.PayType is Food99OrderPaymentMethod.Cash
                    ? orderInfo.Price.CustomerNeedPayingMoney.ToDecimalValue()
                    : null,
                Transaction: null
            )
        ];

        OrderDelivery? orderDelivery = orderInfo.ReceiveAddress is { } delivery
            ? new OrderDelivery(
                PickupCode: null,
                DeliveredBy: OrderDeliveredBy.MERCHANT,
                DeliveryDateTime: orderInfo.ExpectedArrivedEta.ToDateTime(),
                EstimatedDeliveryDateTime: DateTime.Now,
                DeliveryAddress: new Address(
                    City: delivery.City,
                    Complement: delivery.Complement ?? delivery.Reference ?? string.Empty,
                    Country: delivery.CountryCode,
                    District: delivery.District ?? string.Empty,
                    FormattedAddress: delivery.PoiAddress,
                    Number: string.IsNullOrWhiteSpace(delivery.StreetNumber)
                        ? (
                            string.IsNullOrWhiteSpace(delivery.HouseNumber)
                                ? "S/N"
                                : delivery.HouseNumber
                        )
                        : delivery.StreetNumber,
                    PostalCode: delivery.PostalCode?.Replace(" ", string.Empty) ?? string.Empty,
                    Reference: delivery.Reference ?? string.Empty,
                    State: delivery.State ?? string.Empty,
                    Street: delivery.StreetName ?? delivery.PoiDisplayName ?? string.Empty,
                    Coordinates: new AddressCoordinates(
                        Longitude: (decimal)delivery.PoiLat,
                        Latitude: (decimal)delivery.PoiLng
                    )
                )
            )
            : null;

        return new Order(
            OrderId: order.Data.OrderId.ToString(),
            Type: OrderType.DELIVERY,
            DisplayId: Guid.NewGuid().ToString()[..5],
            SourceAppId: nameof(OrderSalesChannel.FOOD99),
            SalesChannel: nameof(OrderSalesChannel.FOOD99),
            VirtualBrand: order.AppShopId,
            CreatedAt: order.Timestamp.ToDateTime(),
            LastEvent: OrderEventType.CREATED,
            OrderTiming: OrderTiming.INSTANT,
            PreparationStartDateTime: DateTime.Now,
            Merchant: new OrderMerchant(
                Id: orderInfo.Shop.AppShopId,
                Name: orderInfo.Shop.ShopName
            ),
            Items: items,
            OtherFees: order.MapFood99OtherFeesToOpenDelivery(),
            Discounts: discounts,
            Total: new OrderTotal(
                Discount: (orderInfo.Price.ItemsDiscount + orderInfo.Price.DeliveryDiscount).ToBrl(),
                ItemsPrice: orderInfo.Price.OrderPrice.ToBrl(),
                OrderAmount: (orderInfo.Price.RealPayPrice ?? 0).ToBrl(),
                OtherFees: orderInfo.Price.DeliveryPrice.ToBrl()
            ),
            Payments: new OrderPayment(
                Prepaid: (orderInfo.Price.RealPayPrice ?? orderInfo.Price.RealPrice) ?? orderInfo.Price.OrderPrice,
                Pending: 0,
                Methods: paymentMethods
            ),
            TaxInvoice: null,
            Customer: new OrderCustomer(
                Id: Guid.NewGuid().ToString()[5..],
                Name: $"{orderInfo.ReceiveAddress.FirstName} {orderInfo.ReceiveAddress.LastName}",
                DocumentNumber: string.Empty,
                Email: string.Empty,
                OrdersCountOnMerchant: 0,
                Phone: new Phone(
                    Extension: string.Empty,
                    Number: orderInfo.ReceiveAddress.Phone
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
            ExternalId: order.Data.OrderId.ToString(),
            Dispute: null
        );
    }

    private static List<OrderDiscountSponsorshipValue> ToSponsorshipValues(this Food99Promotion promo) {
        List<OrderDiscountSponsorshipValue> sponsorshipValues = [];

        if (promo.ShopSubsidePrice > 0) {
            sponsorshipValues.Add(
                new OrderDiscountSponsorshipValue(
                    Amount: promo.ShopSubsidePrice.ToBrl(),
                    Name: OrderSponsorshipName.MERCHANT,
                    Description: OrderSponsorshipName.MERCHANT.GetDescription()
                )
            );
        }

        int integrationAmount = (promo.PromoDiscount - promo.ShopSubsidePrice);

        if (integrationAmount > 0) {
            sponsorshipValues.Add(
                new OrderDiscountSponsorshipValue(
                    Amount: integrationAmount.ToBrl(),
                    Name: OrderSponsorshipName.INTEGRATION,
                    Description: OrderSponsorshipName.INTEGRATION.GetDescription()
                )
            );
        }

        return sponsorshipValues;
    }

    private static string GetDescription(this OrderSponsorshipName sponsor)
    {
        return sponsor switch {
            OrderSponsorshipName.MARKETPLACE => "Desconto patrocinado pelo marketplace",
            OrderSponsorshipName.MERCHANT => "Desconto concedido pela loja",
            OrderSponsorshipName.CHAIN => "Desconto da rede/parceiro",
            OrderSponsorshipName.INTEGRATION => "Desconto pela integração",
            _ => "Desconto aplicado"
        };
    }

    private static OrderSponsorshipName ToSponsorshipName(this Food99OrderPromoType promoType)
    {
        return promoType switch
        {
            Food99OrderPromoType.ShareDeliveryDiscount or
            Food99OrderPromoType.DeliveryMemberDiscount or
            Food99OrderPromoType.OverallOrderCoupon => OrderSponsorshipName.MARKETPLACE,

            Food99OrderPromoType.SaleItemPromotion or
            Food99OrderPromoType.BuyXGetYPromotion => OrderSponsorshipName.MERCHANT,

            Food99OrderPromoType.MinimumOrderDiscount or
            Food99OrderPromoType.FreeDeliveryEvent => OrderSponsorshipName.CHAIN,

            Food99OrderPromoType.NewUserDiscount or
            Food99OrderPromoType.OrderItemsCoupon or
            Food99OrderPromoType.RecurrentUserDiscount or
            Food99OrderPromoType.DidiMembershipDiscount or
            Food99OrderPromoType.DeliveryCoupon => OrderSponsorshipName.INTEGRATION,

            _ => OrderSponsorshipName.MERCHANT
        };
    }

    private static DiscountTarget ToOrder(this Food99OrderPromoType promoType, bool isItemLevel = false)
    {
        return promoType switch
        {
            // Item-level promotions
            Food99OrderPromoType.SaleItemPromotion or
            Food99OrderPromoType.BuyXGetYPromotion => DiscountTarget.ITEM,

            // Delivery fee promotions
            Food99OrderPromoType.FreeDeliveryEvent or
            Food99OrderPromoType.DeliveryCoupon or
            Food99OrderPromoType.DeliveryMemberDiscount or
            Food99OrderPromoType.ShareDeliveryDiscount => DiscountTarget.DELIVERY_FEE,

            // Cart-level promotions
            Food99OrderPromoType.MinimumOrderDiscount or
            Food99OrderPromoType.OverallOrderCoupon or
            Food99OrderPromoType.OrderItemsCoupon => DiscountTarget.CART,

            Food99OrderPromoType.NewUserDiscount or
            Food99OrderPromoType.RecurrentUserDiscount => isItemLevel ? DiscountTarget.ITEM : DiscountTarget.CART,

            _ => DiscountTarget.CART
        };
    }

    private static List<OrderFee> MapFood99OtherFeesToOpenDelivery(this Food99WebhookRequest order)
    {
        List<OrderFee> otherFees = [];

        if (order.Data.OrderInfo.Price.DeliveryPrice > 0)
        {
            otherFees.Add(new OrderFee(
                Name: "TIP",
                Type: FeeType.DELIVERY_FEE,
                ReceivedBy: FeeReceivedBy.MERCHANT,
                Price: order.Data.OrderInfo.Price.DeliveryPrice.ToDecimalValue().ToBrl(),
                ReceiverDocument: "None",
                Observation: "The delivery fee"
            ));
        }

        return otherFees;
    }

    private static decimal ToDecimalValue(this int value)
    {
        return (decimal)value / 100;
    }

    private static Price ToBrl(this decimal price)
    {
        return new Price(
            Value: price,
            Currency: "BRL"
        );
    }

    private static Price ToBrl(this int price)
    {
        return new Price(
            Value: price.ToDecimalValue(),
            Currency: "BRL"
        );
    }

    private static MethodMethod ToOrder(this Food99OrderPaymentMethod method)
    {
        return method switch
        {
            Food99OrderPaymentMethod.OnlinePayment => MethodMethod.CREDIT,          // Pagamento online via cartão
            Food99OrderPaymentMethod.Cash => MethodMethod.CASH,            // Dinheiro
            Food99OrderPaymentMethod.POSDelivery => MethodMethod.DEBIT,           // Cartão na entrega (POS)
            Food99OrderPaymentMethod.Wallet99Food => MethodMethod.DIGITAL_WALLET,  // Carteira 99
            Food99OrderPaymentMethod.PayPalWithoutSecret => MethodMethod.DIGITAL_WALLET, // PayPal (não exige senha)
            Food99OrderPaymentMethod.PayPalWithSecret => MethodMethod.DIGITAL_WALLET,  // PayPal (com senha)
            _ => MethodMethod.OTHER
        };
    }

    private static DateTime ToDateTime(this long time)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time);
        DateTime dateTime = dateTimeOffset.UtcDateTime;
        return dateTime;
    }

    public static OrderDispute ToOrderDispute(this Food99WebhookRequest order)
    {
        if (order.Type is not (Food99Type.OrderCancelApply or Food99Type.OrderRefundApply))
            throw new InvalidOperationException("Invalid Dispute type");

        string disputeId = order.Data.ApplyId!.ToString()?? string.Empty;
        string message = order.Data.ApplyReason ?? string.Empty;
        DateTime createdAt = DateTimeOffset.FromUnixTimeSeconds(order.Timestamp).DateTime;
        List<string>? cancellationReasons = order.Data.ReasonList?.Select(reason => reason.Reason).ToList();

        return order.Type switch {
            Food99Type.OrderCancelApply => new OrderDispute(
                DisputeId: disputeId,
                Message: message,
                Action: "CANCELLATION",
                TimeoutAction: "REJECT_CANCELLATION",
                CreatedAt: createdAt,
                ExpiresAt: createdAt.AddMinutes(10),
                Alternatives: null,
                Evidences: null,
                Items: null,
                Options: null,
                CancellationReasons: cancellationReasons
            ),
            Food99Type.OrderRefundApply => new OrderDispute(
                DisputeId: disputeId,
                Message: message,
                Action: "PROPOSED_AMOUNT_REFUND",
                TimeoutAction: "ACCEPT_CANCELLATION",
                CreatedAt: createdAt,
                ExpiresAt: createdAt.AddHours(24),
                Alternatives: order.Data.BaseReasonList?.Select(
                    reason => new DisputeAlternative(
                        AlternativeId: reason.BaseReasonId,
                        Type: AlternativeType.REFUND,
                        Price: null,
                        AllowedTimesInMinutes: null,
                        AllowedTimesReasons: null
                    )
                ).ToList(),
                Evidences: order.Data.Images?.Select(image => new DisputeEvidence(Url: image)).ToList(),
                Items: null,
                Options: null,
                CancellationReasons: cancellationReasons
            ),
            _ => throw new InvalidOperationException("Invalid Dispute type")
        };
    }

    private static OrderEventType ToOrderEvent(this Food99Type orderEvent, Food99DeliveryStatus? deliveryStatus)
    {
        return orderEvent switch {
            Food99Type.OrderNew => OrderEventType.CREATED,
            Food99Type.OrderFinish => OrderEventType.CONCLUDED,
            Food99Type.OrderCancel or
            Food99Type.OrderPartialCancel => OrderEventType.CANCELLED,
            Food99Type.OrderCancelApply => OrderEventType.CANCELLATION_REQUESTED,
            Food99Type.OrderRefundApply => OrderEventType.DISPUTE_STARTED,
            Food99Type.DeliveryStatus => deliveryStatus switch {
                Food99DeliveryStatus.Assigned => OrderEventType.DISPATCHED,
                Food99DeliveryStatus.ArrivedAtB => OrderEventType.DELIVERYMAN_IN_STORE,
                Food99DeliveryStatus.Taken => OrderEventType.PICKUP_UP,
                Food99DeliveryStatus.ArrivedAtC => OrderEventType.READY_FOR_PICKUP,
                Food99DeliveryStatus.Finish => OrderEventType.DELIVERED,
                Food99DeliveryStatus.Reassigned => OrderEventType.DELIVERYMAN_CHANGED,
                Food99DeliveryStatus.Cancel or
                Food99DeliveryStatus.Aborted => OrderEventType.CANCELLED,
                _ => OrderEventType.CONFIRMED
            },
            _ => OrderEventType.CONFIRMED
        };
    }

    public static OrderUpdate FromFood99(this Food99WebhookRequest request, OrderEventType? eventType)
    {
        return new OrderUpdate(
            OrderId: request.Data.OrderId.ToString(),
            SourceAppId: Food99IntegrationKey.FOOD99,
            Type: eventType?? request.Type.ToOrderEvent(request.Data.DeliveryStatus),
            CreateAt: request.Timestamp.ToDateTime(),
            Dispute: null,
            FromIntegration: true
        );
    }
}
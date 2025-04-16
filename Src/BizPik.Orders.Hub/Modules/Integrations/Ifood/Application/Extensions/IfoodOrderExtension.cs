using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Address;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Discount;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Item;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Merchant;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Payment;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Extensions;

public static class IfoodOrderExtension
{
    public static Order ToOrder(this IfoodOrder order, int companyId)
    {
        List<OrderItem> items = order.Items.Select(item => new OrderItem(
            Id: item.Id,
            Index: item.Index,
            Name: item.Name,
            ExternalCode: item.ExternalCode ?? string.Empty,
            ImageUrl: item.ImageUrl,
            Unit: item.Unit.ToOrderUnit(),
            Ean: item.Ean,
            Quantity: item.Quantity,
            SpecialInstructions: item.Observations,
            UnitPrice: item.UnitPrice.ToBrl(),
            TotalPrice: item.TotalPrice.ToBrl(),
            OptionsPrice: item.OptionsPrice.ToBrl(),
            Options: item.Options?.Select(option  => new OrderItemOption(
                Id: option.Id,
                Index: option.Index,
                Name: option.Name,
                ExternalCode: option.ExternalCode,
                ImageUrl: string.Empty,
                Unit: option.Unit.ToOrderUnit(),
                Ean: option.Ean,
                Quantity: option.Quantity,
                SpecialInstructions: string.Empty,
                UnitPrice: option.UnitPrice.ToBrl(),
                TotalPrice: option.Price.ToBrl()
            )).ToList() ?? []
        )).ToList();

        List<OrderFee> otherFees = order.AdditionalFees?.Select(additionalFee => new OrderFee(
            Name: additionalFee.Description,
            Type: FeeType.SERVICE_FEE,
            ReceivedBy: FeeReceivedBy.MARCHANT,
            Price: additionalFee.Value.ToBrl(),
            ReceiverDocument: "None",
            Observation: additionalFee.FullDescription
        )).ToList() ?? [];

        if (order.Total.DeliveryFee > 0) {
            otherFees.Add(new OrderFee(
                Name: "DELIVERY",
                Type: FeeType.DELIVERY_FEE,
                ReceivedBy: FeeReceivedBy.MARCHANT,
                Price: order.Total.DeliveryFee.ToBrl(),
                ReceiverDocument: "None",
                Observation: "The delivery fee"
            ));
        }

        List<OrderDiscount> discounts = order.Benefits?.Select(benefit => new OrderDiscount(
            Amount: benefit.Value.ToBrl(),
            Target: benefit.Target.ToOrder(),
            TargetId: benefit.TargetId,
            SponsorshipValues: benefit.SponsorshipValues?.Select(sponsorshipValue => new OrderDiscountSponsorshipValue(
                Amount: sponsorshipValue.Value.ToBrl(),
                Name: sponsorshipValue.Name.ToOrder()
            )).ToList()?? []
        )).ToList() ?? [];
        
        List<OrderPaymentMethod> paymentMethods = order.Payments.Methods.Select(method => new OrderPaymentMethod(
            Value: method.Value,
            Currency: "BRL",
            Type: method.Type.ToOrder(), // == "ONLINE") ? "PREPAID" : "PENDING",
            Method: method.Method.ToOrder(),
            Brand: method.Card?.Brand.ToOrder()?? MethodBrand.OTHER,
            MethodInfo: method.Wallet?.Name?? string.Empty,
            ChangeFor: method.Cash?.ChangeFor,
            Transaction: new OrderPaymentMethodTransaction(
                AuthorizationCode: method.Transaction?.AuthorizationCode?? string.Empty,
                AcquirerDocument: method.Transaction?.AcquirerDocument?? string.Empty
            )
        )).ToList();

        OrderDelivery? orderDelivery = order.Delivery is { } delivery
            ? new OrderDelivery(
                DeliveredBy: delivery.DeliveredBy.ToOrder(),
                DeliveryDateTime: delivery.DeliveryDateTime,
                EstimatedDeliveryDateTime: delivery.DeliveryDateTime,
                DeliveryAddress: delivery.DeliveryAddress is { } address
                    ? new Address(
                        City: address.City,
                        Complement: address.Complement,
                        Country: address.Country,
                        District: address.Neighborhood,
                        FormattedAddress: address.FormattedAddress,
                        Number: address.StreetNumber,
                        PostalCode: address.PostalCode,
                        Reference: address.Reference,
                        State: address.State,
                        Street: address.StreetName,
                        Coordinates: new AddressCoordinates(
                            Longitude: address.Coordinates.Longitude,
                            Latitude: address.Coordinates.Latitude
                        )
                    )
                    : null
            )
            : null;

        OrderSchedule? orderSchedule = order.Schedule is { } schedule
          ? new OrderSchedule(
              ScheduledDateTimeEnd: schedule.DeliveryDateTimeEnd,
              ScheduledDateTimeStart: schedule.DeliveryDateTimeStart
          )
          : null;

        OrderTakeout? orderTakeout = order.Takeout is { } takeout
            ? new OrderTakeout(
                Mode: takeout.Mode.ToOrder(),
                TakeoutDateTime: takeout.TakeoutDateTime
            )
            : null;

        OrderIndoor? orderIndoor = order.Indoor is { } indoor
            ? new OrderIndoor(
                Mode: indoor.Mode.ToOrder(),
                IndoorDateTime: indoor.DeliveryDateTime,
                Place: indoor.Table,
                Seat: indoor.Table,
                Tab: null
            )
            : null;

        return new Order(
            OrderId: order.Id,
            Type: order.orderType.ToOrder(),
            DisplayId: order.DisplayId,
            SourceAppId: OrderSalesChannel.IFOOD.ToString(),
            SalesChannel: order.SalesChannel.ToString(),
            VirtualBrand: order.Merchant.Id,
            CreatedAt: order.CreatedAt,
            LastEvent: OrderEventType.CREATED,
            OrderTiming: order.OrderTiming.ToOrder(),
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
            ExtraInfo: order.ExtraInfo,
            CompanyId: companyId,
            OrderDisplayId: Guid.NewGuid().ToString()[..5],
            ExternalId: order.Id
        );
    }

    private static OrderUnit ToOrderUnit(this Unit unit) {
        return unit switch {
            Unit.UN => OrderUnit.UN,
            Unit.G or Unit.KG => OrderUnit.KG,
            Unit.L or Unit.ML => OrderUnit.L,
            _ => OrderUnit.UN,
        };
    }

    private static Price ToBrl(this decimal price)
    {
        return new Price(
            Value: price,
            Currency: "BRL"
        );
    }

    private static OrderPriority ToOrderPriority(this Mode orderPriority)
    {
        return orderPriority switch {
            Mode.DEFAULT => OrderPriority.PRIORITY5,
            Mode.EXPRESS => OrderPriority.PRIORITY4,
            Mode.HIGH_DENSITY => OrderPriority.PRIORITY3,
            Mode.TURBO => OrderPriority.PRIORITY2,
            Mode.PRIORITY => OrderPriority.PRIORITY1,
            _ => OrderPriority.PRIORITY5
        };
    }

    private static OrderTiming ToOrder(this OrderTimingIfood orderTiming)
    {
        return orderTiming switch {
            OrderTimingIfood.IMMEDIATE => OrderTiming.INSTANT,
            OrderTimingIfood.SCHEDULED => OrderTiming.SCHEDULED,
            _ => OrderTiming.INSTANT
        };
    }

    private static DiscountTarget ToOrder(this Target target)
    {
        return target switch {
            Target.CART => DiscountTarget.CART,
            Target.ITEM => DiscountTarget.ITEM,
            Target.DELIVERT_FEE => DiscountTarget.DELIVERY_FEE,
            _ => DiscountTarget.CART,
        };
    }

    private static OrderSponsorshipName ToOrder(this SponsorshipName sponsorshipName)
    {
        return sponsorshipName switch {
            SponsorshipName.IFOOD => OrderSponsorshipName.MERCHANT,
            SponsorshipName.EXTERNAL => OrderSponsorshipName.MARKETPLACE,
            SponsorshipName.MERCHANT => OrderSponsorshipName.MERCHANT,
            SponsorshipName.CHAIN => OrderSponsorshipName.CHAIN,
            _ => OrderSponsorshipName.MERCHANT
        };
    }

    private static MethodType ToOrder(this IfoodMethodType methodType)
    {
        return methodType switch {
            IfoodMethodType.ONLINE => MethodType.PREPAID,
            IfoodMethodType.OFFLINE => MethodType.PENDING,
            _ => MethodType.PREPAID,
        };
    }

    private static MethodMethod ToOrder(this Method method)
    {
        return method switch
        {
            Method.CASH => MethodMethod.CASH,
            Method.CREDIT => MethodMethod.CREDIT,
            Method.DEBIT => MethodMethod.DEBIT,
            Method.MEAL_VOUCHER => MethodMethod.MEAL_VOUCHER,
            Method.FOOD_VOUCHER => MethodMethod.FOOD_VOUCHER,
            Method.GIFT_CARD => MethodMethod.REDEEM,
            Method.DIGITAL_WALLET => MethodMethod.DIGITAL_WALLET,
            Method.PIX => MethodMethod.PIX,
            Method.OTHER => MethodMethod.OTHER,
            _ => MethodMethod.OTHER
        };
    }

    private static MethodBrand ToOrder(this string brand)
    {
        bool isParseable = Enum.TryParse(brand.ToUpper(), out MethodBrand methodBrand);
        return isParseable ? methodBrand : MethodBrand.OTHER;
    }

    private static OrderDeliveredBy ToOrder(this DeliveredBy deliveredBy)
    {
        return deliveredBy switch {
            DeliveredBy.IFOOD => OrderDeliveredBy.MERCHANT,
            DeliveredBy.MERCHANT => OrderDeliveredBy.MERCHANT,
            _ => OrderDeliveredBy.MARKETPLACE
        };
    }

    private static OrderIndoorMode ToOrder(this IndoorMode mode)
    {
        return mode switch {
            IndoorMode.TABLE => OrderIndoorMode.TAB,
            IndoorMode.DEFAULT => OrderIndoorMode.DEFAULT,
            _ => OrderIndoorMode.PLACE
        };
    }

    private static OrderType ToOrder(this IfoodOrderType orderType)
    {
        return orderType switch
        {
            IfoodOrderType.DELIVERY => OrderType.DELIVERY,
            IfoodOrderType.INDOOR => OrderType.INDOOR,
            IfoodOrderType.TAKEOUT => OrderType.TAKEOUT,
            _ => OrderType.DELIVERY
        };
    }

    private static OrderTakeoutMode ToOrder(this TakeoutMode takeoutMode)
    {
        return takeoutMode switch
        {
            TakeoutMode.DEFAULT => OrderTakeoutMode.DEFAULT,
            TakeoutMode.PICKUP_AREA => OrderTakeoutMode.PICKUP_AREA,
            _ => OrderTakeoutMode.DEFAULT
        };
    }

    public static OrderUpdateStatus FromIfood(this IfoodWebhookRequest request)
    {
        return new OrderUpdateStatus(
            OrderId: request.OrderId,
            SourceAppId: "ifood",
            Type: OrderEventType.CONFIRMED,
            CreateAt: request.CreatedAt,
            FromIntegration: true
        );
    }
}
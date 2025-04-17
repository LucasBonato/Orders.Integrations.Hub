using System.Text.Json.Serialization;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Discount;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Item;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Merchant;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Payment;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;

public record Order(
    [property: JsonPropertyName("orderId")]                     string OrderId,
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))]                        OrderType Type,
    [property: JsonPropertyName("displayId")]                   string DisplayId,
    [property: JsonPropertyName("sourceAppId")]                 string SourceAppId,
    [property: JsonPropertyName("salesChannel")]                string? SalesChannel,
    [property: JsonPropertyName("virtualBrand")]                string? VirtualBrand,
    [property: JsonPropertyName("createdAt")]                   DateTime CreatedAt,
    [property: JsonPropertyName("lastEvent")] [property: JsonConverter(typeof(JsonStringEnumConverter))]                  OrderEventType LastEvent,
    [property: JsonPropertyName("orderTiming")] [property: JsonConverter(typeof(JsonStringEnumConverter))]                OrderTiming OrderTiming,
    [property: JsonPropertyName("preparationStartDateTime")]    DateTime PreparationStartDateTime,
    [property: JsonPropertyName("merchant")]                    OrderMerchant Merchant,
    [property: JsonPropertyName("items")]                       IReadOnlyList<OrderItem> Items,
    [property: JsonPropertyName("otherFees")]                   IReadOnlyList<OrderFee> OtherFees,
    [property: JsonPropertyName("discounts")]                   IReadOnlyList<OrderDiscount> Discounts,
    [property: JsonPropertyName("total")]                       OrderTotal Total,
    [property: JsonPropertyName("payments")]                    OrderPayment? Payments,
    [property: JsonPropertyName("taxInvoice")]                  OrderTaxInvoice? TaxInvoice,
    [property: JsonPropertyName("customer")]                    OrderCustomer? Customer,
    [property: JsonPropertyName("schedule")]                    OrderSchedule? Schedule,
    [property: JsonPropertyName("orderPriority")]               OrderPriority? OrderPriority,
    [property: JsonPropertyName("delivery")]                    OrderDelivery? Delivery,
    [property: JsonPropertyName("takeout")]                     OrderTakeout? Takeout,
    [property: JsonPropertyName("indoor")]                      OrderIndoor? Indoor,
    [property: JsonPropertyName("sendPreparing")]               bool? SendPreparing,
    [property: JsonPropertyName("sendDelivered")]               bool? SendDelivered,
    [property: JsonPropertyName("sendPickedUp")]                bool? SendPickedUp,
    [property: JsonPropertyName("sendTracking")]                bool? SendTracking,
    [property: JsonPropertyName("extraInfo")]                   string ExtraInfo,

    [property: JsonPropertyName("externalId")]                  string ExternalId,
    [property: JsonPropertyName("orderDisplayId")]              string? OrderDisplayId,
    [property: JsonPropertyName("companyId")]                   int CompanyId
);
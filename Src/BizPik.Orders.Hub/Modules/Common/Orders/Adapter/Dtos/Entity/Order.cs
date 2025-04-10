using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Discount;
using BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Item;
using BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Merchant;
using BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Payment;
using BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Enums;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

public record Order(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("externalId")] string ExternalId,
    [property: JsonPropertyName("orderDisplayId")] string OrderDisplayId,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("displayId")] string DisplayId,
    [property: JsonPropertyName("sourceAppId")] string SourceAppId,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("lastEvent")] string LastEvent,
    [property: JsonPropertyName("orderTiming")] string OrderTiming,
    [property: JsonPropertyName("preparationStartDateTime")] DateTime PreparationStartDateTime,
    [property: JsonPropertyName("merchant")] OrderMerchant Merchant,
    [property: JsonPropertyName("items")] IReadOnlyList<OrderItem> Items,
    [property: JsonPropertyName("otherFees")] IReadOnlyList<OrderFee> OtherFees,
    [property: JsonPropertyName("discounts")] IReadOnlyList<OrderDiscount> Discounts,
    [property: JsonPropertyName("total")] OrderTotal Total,
    [property: JsonPropertyName("payments")] OrderPayment Payments,
    [property: JsonPropertyName("customer")] OrderCustomer Customer,
    [property: JsonPropertyName("schedule")] OrderSchedule? Schedule,
    [property: JsonPropertyName("delivery")] OrderDelivery? Delivery,
    [property: JsonPropertyName("indoor")] OrderIndoor? Indoor,
    [property: JsonPropertyName("orderPriority")] string? OrderPriority,
    [property: JsonPropertyName("sendDelivered")] bool SendDelivered,
    [property: JsonPropertyName("sendTracking")] bool SendTracking,
    [property: JsonPropertyName("extraInfo")] string ExtraInfo,
    [property: JsonPropertyName("companyId")] int CompanyId
);

public class Order
{
     #region  OpenDelivery

    /// <summary>
    /// Unique identifier of the Order
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// <c>DELIVERY</c> - for orders that will be delivered to the customer's address.<br/>
    /// <c>TAKEOUT</c> - orders that will be picked up at the establishment by the customer<br/>
    /// <c>INDOOR</c> - orders that will be consumed inside the establishment.
    /// </summary>
    public OrderType Type { get; set; }
    public string DisplayId { get; set; } = string.Empty;
    public string SourceAppId { get; set; } = string.Empty;
    public string SalesChannel { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public OrderEventType LastEvent { get; set; }
    public OrderTiming OrderTiming { get; set; }
    public DateTime PreparationStartDateTime { get; set; }
    public OrderMerchant Merchant { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public List<OrderFee> OtherFees { get; set; } = new();
    public List<OrderDiscount> Discounts { get; set; } = new();
    public OrderTotal Total { get; set; } = default!;
    public OrderPayment? Payments { get; set; }
    public OrderTaxInvoice? TaxInvoices { get; set; }
    public OrderCustomer? Customer { get; set; }
    public OrderSchedule? Schedule { get; set; }
    public OrderPriority OrderPriority { get; set; }
    public OrderDelivery? Delivery { get; set; }
    public OrderTakeout? TakeOut { get; set; }
    public OrderIndoor? Indoor { get; set; }
    public bool SendPreparing { get; set; }
    public bool SendDelivered { get; set; }
    public bool SendPickedUp { get; set; }
    public bool SendTracking { get; set; }
    public string ExtraInfo { get; set; } = "";

    #endregion

    #region BizPik

    public string ExternalId { get; set; } = string.Empty;
    public int CompanyId { get; set; }

    #endregion
}
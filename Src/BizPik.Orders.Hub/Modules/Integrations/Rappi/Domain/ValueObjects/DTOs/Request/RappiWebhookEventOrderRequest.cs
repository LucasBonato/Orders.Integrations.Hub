using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookEventOrderRequest(
    [property: JsonPropertyName("event")] RappiWebhookOrderEvent OrderEvent,
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("store_id")] string StoreId,
    [property: JsonPropertyName("event_time")] DateTime? EventTime,
    [property: JsonPropertyName("additional_information")] RappiWebhookEventAdditionalInformation? AdditionalInformation
);
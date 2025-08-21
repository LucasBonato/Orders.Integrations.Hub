using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookEventOrderRequest(
    RappiWebhookOrderEvent OrderEvent,
    string OrderId,
    string StoreId,
    DateTime? EventTime,
    RappiWebhookEventAdditionalInformation? AdditionalInformation
);
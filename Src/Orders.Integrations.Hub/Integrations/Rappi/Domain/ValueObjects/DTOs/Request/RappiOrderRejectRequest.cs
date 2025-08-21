using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiOrderRejectRequest(
    string Reason,
    List<string>? ItemIds,
    List<string>? ItemSkus,
    RappiOrderCancelType CancelType
);
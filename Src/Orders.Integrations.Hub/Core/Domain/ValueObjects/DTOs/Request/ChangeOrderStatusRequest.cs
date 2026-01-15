using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;

public record ChangeOrderStatusRequest(
    string OrderId,
    string ExternalId,
    string MerchantId,
    OrderEventType Status,
    IntegrationKey Integration,
    string? CancellationReason,
    CancellationMetadata? CancellationMetadata
);
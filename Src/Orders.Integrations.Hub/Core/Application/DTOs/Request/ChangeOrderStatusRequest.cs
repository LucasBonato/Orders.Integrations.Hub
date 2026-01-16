using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.Core.Application.DTOs.Request;

public record ChangeOrderStatusRequest(
    string OrderId,
    string ExternalId,
    string MerchantId,
    OrderEventType Status,
    IntegrationKey Integration,
    string? CancellationReason,
    CancellationMetadata? CancellationMetadata
);
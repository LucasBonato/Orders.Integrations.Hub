using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;

public record OrderUpdate(
    string OrderId,
    IntegrationKey SourceAppId,
    OrderEventType Type,
    DateTime CreateAt,
    OrderDispute? Dispute,
    bool FromIntegration
);
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Application.DTOs;

public record OrderUpdate(
    string OrderId,
    string SourceAppId,
    OrderEventType Type,
    DateTime CreateAt,
    OrderDispute? Dispute,
    bool FromIntegration
);
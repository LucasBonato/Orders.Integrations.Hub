using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.Core.Application.DTOs.Request;

public record OrderCancellationReasonRequest(
    string OrderId,
    IntegrationKey Integration
);
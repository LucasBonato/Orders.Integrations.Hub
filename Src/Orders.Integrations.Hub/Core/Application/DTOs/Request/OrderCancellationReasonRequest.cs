namespace Orders.Integrations.Hub.Core.Application.DTOs.Request;

public record OrderCancellationReasonRequest(
    string OrderId,
    IntegrationKey Integration
);
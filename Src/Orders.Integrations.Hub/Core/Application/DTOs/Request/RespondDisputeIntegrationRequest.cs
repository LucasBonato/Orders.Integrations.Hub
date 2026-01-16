using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.Core.Application.DTOs.Request;

public record RespondDisputeIntegrationRequest(
    string DisputeId,
    IntegrationKey Integration,
    DisputeResponseType Type,
    string? AlternativeId,
    RespondDisputeResponse DisputeResponse
);
using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;

public record RespondDisputeIntegrationRequest(
    string DisputeId,
    IntegrationKey Integration,
    DisputeResponseType Type,
    string? AlternativeId,
    RespondDisputeResponse DisputeResponse
);
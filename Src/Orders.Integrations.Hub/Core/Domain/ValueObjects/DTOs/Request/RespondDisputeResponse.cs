using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;

// The name ends with response because it's the response to the dispute
public record RespondDisputeResponse(
    string? Reason,
    string? DetailsReason,
    string? Type,
    Price? Price,
    int? AdditionalTimeInMinutes,
    string? AdditionalTimeReason
);
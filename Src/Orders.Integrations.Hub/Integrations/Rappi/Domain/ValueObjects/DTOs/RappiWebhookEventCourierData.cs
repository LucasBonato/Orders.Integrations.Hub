namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookEventCourierData(
    int? Id,
    string? Phone,
    string? FullName,
    string? LastName,
    string? FirstName,
    string? ProfilePic
);
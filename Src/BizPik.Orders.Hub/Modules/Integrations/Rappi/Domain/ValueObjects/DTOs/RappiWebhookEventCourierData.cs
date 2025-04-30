using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookEventCourierData(
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonPropertyName("phone")] string? Phone,
    [property: JsonPropertyName("full_name")] string? FullName,
    [property: JsonPropertyName("last_name")] string? LastName,
    [property: JsonPropertyName("first_name")] string? FirstName,
    [property: JsonPropertyName("profile_pic")] string? ProfilePic
);
using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodCancellationReasonResponse(
    [property: JsonPropertyName("cancelCodeId")] string CancelCodeId,
    [property: JsonPropertyName("description")] string Description
);
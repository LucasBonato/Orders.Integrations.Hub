using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodDefaultError(
    [property: JsonPropertyName("error")] IfoodError? Error
);
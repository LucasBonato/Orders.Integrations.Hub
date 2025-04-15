using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodDefaultError(
    [property: JsonPropertyName("error")] IfoodError? Error
);
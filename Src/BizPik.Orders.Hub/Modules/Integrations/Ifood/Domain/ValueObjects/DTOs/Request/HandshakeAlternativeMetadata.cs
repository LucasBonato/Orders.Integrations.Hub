using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Handshake;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record HandshakeAlternativeMetadata(
    [property: JsonPropertyName("amount")] Amount? Amount,
    [property: JsonPropertyName("additionalTimeInMinutes")] int? AdditionalTimeInMinutes,
    [property: JsonPropertyName("additionalTimeReason")] [property: JsonConverter(typeof(JsonStringEnumConverter))] NegotiationReasons? AdditionalTimeReason
);
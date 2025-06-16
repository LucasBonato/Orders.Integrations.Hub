using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;

public record CompanySocial(
    [property: JsonProperty("id")]
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonProperty("socialId")]
    [property: JsonPropertyName("socialId")] int? SocialId,
    [property: JsonProperty("companyId")]
    [property: JsonPropertyName("companyId")] int? CompanyId,
    [property: JsonProperty("name")]
    [property: JsonPropertyName("name")] string Name,
    [property: JsonProperty("social")]
    [property: JsonPropertyName("social")] Social Social
);
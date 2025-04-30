using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

public record User(
    [property: JsonProperty("id")]
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonProperty("contactId")]
    [property: JsonPropertyName("contactId")] string ContactId,
    [property: JsonProperty("companyId")]
    [property: JsonPropertyName("companyId")] int? CompanyId,
    [property: JsonProperty("roleId")]
    [property: JsonPropertyName("roleId")] int? RoleId,
    [property: JsonProperty("email")]
    [property: JsonPropertyName("email")] string Email,
    [property: JsonProperty("creatorId")]
    [property: JsonPropertyName("creatorId")] int? CreatorId,
    [property: JsonProperty("encryptKey")]
    [property: JsonPropertyName("encryptKey")] string EncryptKey,
    [property: JsonProperty("refreshToken")]
    [property: JsonPropertyName("refreshToken")] string RefreshToken,
    [property: JsonProperty("refreshTokenExpirationDate")]
    [property: JsonPropertyName("refreshTokenExpirationDate")] DateTime? RefreshTokenExpirationDate,
    [property: JsonProperty("profileBackground")]
    [property: JsonPropertyName("profileBackground")] object ProfileBackground,
    [property: JsonProperty("fbToken")]
    [property: JsonPropertyName("fbToken")] string FbToken,
    [property: JsonProperty("fbUserId")]
    [property: JsonPropertyName("fbUserId")] string FbUserId,
    [property: JsonProperty("userAccountStatusId")]
    [property: JsonPropertyName("userAccountStatusId")] int? UserAccountStatusId,
    [property: JsonProperty("statusId")]
    [property: JsonPropertyName("statusId")] int? StatusId,
    [property: JsonProperty("isConnected")]
    [property: JsonPropertyName("isConnected")] bool? IsConnected,
    [property: JsonProperty("docNumber")]
    [property: JsonPropertyName("docNumber")] object DocNumber,
    [property: JsonProperty("creationDate")]
    [property: JsonPropertyName("creationDate")] DateTime? CreationDate,
    [property: JsonProperty("contact")]
    [property: JsonPropertyName("contact")] object Contact,
    [property: JsonProperty("role")]
    [property: JsonPropertyName("role")] object Role
);
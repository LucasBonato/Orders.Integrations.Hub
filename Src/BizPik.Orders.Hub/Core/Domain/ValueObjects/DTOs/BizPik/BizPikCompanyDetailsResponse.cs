using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;

public record BizPikCompanyDetailsResponse(
    [property: JsonProperty("id")]
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonProperty("name")]
    [property: JsonPropertyName("name")] string Name,
    [property: JsonProperty("pictureUrl")]
    [property: JsonPropertyName("pictureUrl")] object PictureUrl,
    [property: JsonProperty("pictureFileName")]
    [property: JsonPropertyName("pictureFileName")] object PictureFileName,
    [property: JsonProperty("docNumber")]
    [property: JsonPropertyName("docNumber")] string DocNumber,
    [property: JsonProperty("statusId")]
    [property: JsonPropertyName("statusId")] int? StatusId,
    [property: JsonProperty("certificateName")]
    [property: JsonPropertyName("certificateName")] string CertificateName,
    [property: JsonProperty("certificateFile")]
    [property: JsonPropertyName("certificateFile")] string CertificateFile,
    [property: JsonProperty("certificateData")]
    [property: JsonPropertyName("certificateData")] string CertificateData,
    [property: JsonProperty("certificatePassword")]
    [property: JsonPropertyName("certificatePassword")] string CertificatePassword,
    [property: JsonProperty("certificateExpirationDate")]
    [property: JsonPropertyName("certificateExpirationDate")] DateTime? CertificateExpirationDate,
    [property: JsonProperty("website")]
    [property: JsonPropertyName("website")] string Website,
    [property: JsonProperty("address1")]
    [property: JsonPropertyName("address1")] string Address1,
    [property: JsonProperty("address2")]
    [property: JsonPropertyName("address2")] object Address2,
    [property: JsonProperty("address3")]
    [property: JsonPropertyName("address3")] object Address3,
    [property: JsonProperty("city")]
    [property: JsonPropertyName("city")] string City,
    [property: JsonProperty("state")]
    [property: JsonPropertyName("state")] string State,
    [property: JsonProperty("country")]
    [property: JsonPropertyName("country")] string Country,
    [property: JsonProperty("postalCode")]
    [property: JsonPropertyName("postalCode")] string PostalCode,
    [property: JsonProperty("addressNumber")]
    [property: JsonPropertyName("addressNumber")] int? AddressNumber,
    [property: JsonProperty("district")]
    [property: JsonPropertyName("district")] string District,
    [property: JsonProperty("addressComplement")]
    [property: JsonPropertyName("addressComplement")] string AddressComplement,
    [property: JsonProperty("taxpayerSecurityCodeId")]
    [property: JsonPropertyName("taxpayerSecurityCodeId")] string TaxpayerSecurityCodeId,
    [property: JsonProperty("taxpayerSecurityCode")]
    [property: JsonPropertyName("taxpayerSecurityCode")] string TaxpayerSecurityCode,
    [property: JsonProperty("invoiceSeries")]
    [property: JsonPropertyName("invoiceSeries")] int? InvoiceSeries,
    [property: JsonProperty("invoiceInitialNumber")]
    [property: JsonPropertyName("invoiceInitialNumber")] int? InvoiceInitialNumber,
    [property: JsonProperty("cityCode")]
    [property: JsonPropertyName("cityCode")] int? CityCode,
    [property: JsonProperty("stateRegNumber")]
    [property: JsonPropertyName("stateRegNumber")] string StateRegNumber,
    [property: JsonProperty("taxScheme")]
    [property: JsonPropertyName("taxScheme")] int? TaxScheme,
    [property: JsonProperty("allowPlanChange")]
    [property: JsonPropertyName("allowPlanChange")] bool? AllowPlanChange,
    [property: JsonProperty("prompt")]
    [property: JsonPropertyName("prompt")] string Prompt,
    [property: JsonProperty("infoPrompt")]
    [property: JsonPropertyName("infoPrompt")] string InfoPrompt,
    [property: JsonProperty("businessCategory")]
    [property: JsonPropertyName("businessCategory")] string BusinessCategory,
    [property: JsonProperty("accountableUserId")]
    [property: JsonPropertyName("accountableUserId")] int? AccountableUserId,
    [property: JsonProperty("builderWorkspaceId")]
    [property: JsonPropertyName("builderWorkspaceId")] string BuilderWorkspaceId,
    [property: JsonProperty("bucketPicture")]
    [property: JsonPropertyName("bucketPicture")] object BucketPicture,
    [property: JsonProperty("status")]
    [property: JsonPropertyName("status")] object Status,
    [property: JsonProperty("companySocials")]
    [property: JsonPropertyName("companySocials")] IReadOnlyList<CompanySocial> CompanySocials,
    [property: JsonProperty("plan")]
    [property: JsonPropertyName("plan")] object Plan,
    [property: JsonProperty("plugin")]
    [property: JsonPropertyName("plugin")] object Plugin,
    [property: JsonProperty("payment")]
    [property: JsonPropertyName("payment")] object Payment,
    [property: JsonProperty("users")]
    [property: JsonPropertyName("users")] IReadOnlyList<User> Users,
    [property: JsonProperty("integration")]
    [property: JsonPropertyName("integration")] ChangeEndpointRequest Integration
);
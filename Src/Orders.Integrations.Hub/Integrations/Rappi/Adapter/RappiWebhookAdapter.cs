using System.Text;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapter;

public class RappiWebhookAdapter {
    public static async Task<IResult> CreateOrder(
        [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer serializer,
        [FromServices] IOrderCreateUseCase<RappiOrder> orderCreate,
        [FromServices] IIntegrationContext integrationContext,
        [FromServices] IInternalClient internalClient,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiOrder request = await HandleSignature<RappiOrder>(integrationContext, logger, serializer, internalClient, context);
        
        await orderCreate.ExecuteAsync(request);

        return Created();
    }

    public static async Task<IResult> CancelOrder(
        [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer serializer,
        [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
        [FromServices] IIntegrationContext integrationContext,
        [FromServices] IInternalClient internalClient,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = await HandleSignature<RappiWebhookEventOrderRequest>(integrationContext, logger, serializer, internalClient, context);
        await orderUpdate.ExecuteAsync(request);
        return Accepted();
    }

    public static async Task<IResult> PatchOrder(
        [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer serializer,
        [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
        [FromServices] IIntegrationContext integrationContext,
        [FromServices] IInternalClient internalClient,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = await HandleSignature<RappiWebhookEventOrderRequest>(integrationContext, logger, serializer, internalClient, context);
        await orderUpdate.ExecuteAsync(request);
        return Accepted();
    }

    public static async Task<IResult> PingStore(
        [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer serializer,
        [FromServices] IIntegrationContext integrationContext,
        [FromServices] IInternalClient internalClient,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiWebhookPingRequest _ = await HandleSignature<RappiWebhookPingRequest>(integrationContext, logger, serializer, internalClient, context);

        // TODO - Manage better this response finding somewhere if the store is on

        return Ok(new RappiWebhookPingResponse(
            Status: "Ok",
            Description: "Store on"
        ));
    }

    /// <summary>
    /// This will valid the signature <c>Rappi-Signature</c>
    /// </summary>
    /// <param name="internalClient">Client to get info of the integration</param>
    /// <param name="context">The HttpContext of the request</param>
    /// <param name="integrationContext">Context of the integration for the request scope</param>
    /// <param name="logger">A logger</param>
    /// <param name="serializer">A serializer with rappi configs</param>
    /// <returns>The Body of the request formatted as a RappiOrder</returns>
    private static async Task<TRequest> HandleSignature<TRequest>(
        IIntegrationContext integrationContext,
        ILogger<RappiWebhookAdapter> logger,
        ICustomJsonSerializer serializer,
        IInternalClient internalClient,
        HttpContext context
    ) {
        string body;
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true)) {
            body = await reader.ReadToEndAsync();
        }

        logger.LogInformation("[INFO] - RappiSignatureValidator - Request Body: {body}", body);

        string? signature = context.Request.Headers["Rappi-Signature"].FirstOrDefault();

        TRequest request = serializer.Deserialize<TRequest>(body)!;

        if (signature == null)
        {
            logger.LogWarning("[WARN] - Signature Header missing");
            context.Response.StatusCode = 401;
            throw new Exception("Missing Rappi-Signature");
        }

        Dictionary<string, string> timestampAndSign = GetTimestampAndSign(signature);

        var bodyDeserialized = serializer.Deserialize<object>(body);
        body = serializer.Serialize(bodyDeserialized);

        body = $"{timestampAndSign["t"]}.{body}";

        signature = timestampAndSign["sign"];

        string merchantId = request switch {
            RappiOrder rappiOrder => rappiOrder.Store.ExternalId,
            RappiWebhookEventOrderRequest rappiOrderEvent => rappiOrderEvent.StoreId,
            RappiWebhookPingRequest rappiPing => rappiPing.StoreId.ToString(),
            _ => throw new InvalidOperationException()
        };

        integrationContext.Integration = (await internalClient.GetIntegrationByExternalId(merchantId)).ResolveRappi();
        integrationContext.MerchantId = merchantId;

        string secret = integrationContext.Integration.ClientSecret;

        if (!signature.IsSignatureValid(secret, body)) {
            logger.LogWarning("[WARN] - Invalid signature.");
            context.Response.StatusCode = 403;
            throw new Exception("Invalid Signature");
        }

        return request;
    }

    /// <summary>
    /// Split the Header <c>Rappi-Signature</c> into a dictionary <br/>
    /// getting the timestamp <c>t</c> and the signature <c>sign</c>
    /// </summary>
    /// <param name="header">The <c>Rappi-Signature</c> received.</param>
    /// <returns>A Dictionary with the timestamp <c>t</c> and signature <c>sign</c></returns>
    /// <exception cref="ArgumentException">Occurs when there isn't <c>t</c> or <c>sign</c> keys</exception>
    private static Dictionary<string, string> GetTimestampAndSign(string header) {
        string[] timestampAndSignature = header.Split(",");

        Dictionary<string, string> response = timestampAndSignature
            .Select(item => item.Split("="))
            .Where(timestampOrSignatureDivided => timestampOrSignatureDivided[0] == "t" || timestampOrSignatureDivided[0] == "sign")
            .ToDictionary(timestampOrSignatureDivided => timestampOrSignatureDivided[0], timestampOrSignatureDivided => timestampOrSignatureDivided[1]);

        if (response.Count < 1)
            throw new ArgumentException("O Header não possui 't' e 'sign'.");
        return response;
    }
}
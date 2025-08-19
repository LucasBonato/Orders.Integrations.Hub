using System.Text;

using FastEndpoints;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapter;

public class RappiWebhookAdapter {
    public static async Task<IResult> CreateOrder(
        [FromKeyedServices(OrderIntegration.RAPPI)] ICustomJsonSerializer serializer,
        [FromServices] IOrderCreateUseCase<RappiOrder> orderCreate,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiOrder request = await HandleSignature<RappiOrder>(context, logger, serializer);
        
        await orderCreate.ExecuteAsync(request);

        return Created();
    }

    public static async Task<IResult> AutoAcceptOrder(
        [FromKeyedServices(OrderIntegration.RAPPI)] ICustomJsonSerializer serializer,
        [FromServices] IOrderCreateUseCase<RappiOrder> orderCreate,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiOrder request = await HandleSignature<RappiOrder>(context, logger, serializer);
        
        await orderCreate.ExecuteAsync(request);
        
        await new SendNotificationEvent(
            Message: request.FromRappi(OrderEventType.CONFIRMED),
            TopicArn: null
        ).PublishAsync();
        
        return Created();
    }

    public static async Task<IResult> CancelOrder(
        [FromKeyedServices(OrderIntegration.RAPPI)] ICustomJsonSerializer serializer,
        [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = await HandleSignature<RappiWebhookEventOrderRequest>(context, logger, serializer);
        await orderUpdate.ExecuteAsync(request);
        return Accepted();
    }

    public static async Task<IResult> PatchOrder(
        [FromKeyedServices(OrderIntegration.RAPPI)] ICustomJsonSerializer serializer,
        [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = await HandleSignature<RappiWebhookEventOrderRequest>(context, logger, serializer);
        await orderUpdate.ExecuteAsync(request);
        return Accepted();
    }

    public static async Task<IResult> PingStore(
        [FromKeyedServices(OrderIntegration.RAPPI)] ICustomJsonSerializer serializer,
        ILogger<RappiWebhookAdapter> logger,
        HttpContext context
    ) {
        RappiWebhookPingRequest request = await HandleSignature<RappiWebhookPingRequest>(context, logger, serializer);

        // TODO - Manage better this response finding somewhere if the store is on

        return Ok(new RappiWebhookPingResponse(
            Status: "Ok",
            Description: "Store on"
        ));
    }

    /// <summary>
    /// This will valid the signature <c>Rappi-Signature</c>
    /// </summary>
    /// <param name="context">The HttpContext of the request</param>
    /// <param name="logger">A logger</param>
    /// <param name="serializer">A serializer with rappi configs</param>
    /// <returns>The Body of the request formatted as a RappiOrder</returns>
    private static async Task<TRequest> HandleSignature<TRequest>(
        HttpContext context,
        ILogger<RappiWebhookAdapter> logger,
        ICustomJsonSerializer serializer
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

        string secret = AppEnv.INTEGRATIONS.RAPPI.CLIENT.SECRET.NotNullEnv();

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
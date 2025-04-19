using System.Text;
using System.Text.Json;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Modules.Integrations.Common.Validators;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Extensions;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

using FastEndpoints;

using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Adapter;

public abstract class RappiWebhookAdapterLog;

public static class RappiWebhookAdapter {
    public static async Task<IResult> CreateOrder(
        [FromServices] ICreateOrderUseCase<RappiOrder> createOrder,
        ILogger<RappiWebhookAdapterLog> logger,
        HttpContext context
    ) {
        RappiOrder request = await HandleSignature<RappiOrder>(context, logger);
        
        await createOrder.ExecuteAsync(request);

        return Created();
    }

    public static async Task<IResult> AutoAcceptOrder(
        [FromServices] ICreateOrderUseCase<RappiOrder> createOrder,
        ILogger<RappiWebhookAdapterLog> logger,
        HttpContext context
    ) {
        RappiOrder request = await HandleSignature<RappiOrder>(context, logger);
        
        await createOrder.ExecuteAsync(request);
        
        await new SendNotificationEvent(
            Message: request.FromRappi(OrderEventType.CONFIRMED),
            TopicArn: null
        ).PublishAsync();
        
        return Created();
    }

    public static async Task<IResult> CancelOrder(
        [FromServices] IUpdateOrderStatusUseCase<RappiWebhookEventOrderRequest> updateOrder,
        ILogger<RappiWebhookAdapterLog> logger,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = await HandleSignature<RappiWebhookEventOrderRequest>(context, logger);
        await updateOrder.ExecuteAsync(request);
        return Accepted();
    }

    public static async Task<IResult> PatchOrder(
        [FromServices] IUpdateOrderStatusUseCase<RappiWebhookEventOrderRequest> updateOrder,
        ILogger<RappiWebhookAdapterLog> logger,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = await HandleSignature<RappiWebhookEventOrderRequest>(context, logger);
        await updateOrder.ExecuteAsync(request);
        return Accepted();
    }

    public static async Task<IResult> PingStore(
        ILogger<RappiWebhookAdapterLog> logger,
        HttpContext context
    ) {
        RappiWebhookPingRequest request = await HandleSignature<RappiWebhookPingRequest>(context, logger);

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
    /// <returns>The Body of the request formatted as a RappiOrder</returns>
    private static async Task<TRequest> HandleSignature<TRequest>(HttpContext context, ILogger<RappiWebhookAdapterLog> logger) {
        var request = context.Request;

        if (!request.Headers.TryGetValue("Rappi-Signature", out var signatureHeader)) {
            context.Response.StatusCode = 401;
            logger.LogError("Signature Header missing");
            await context.Response.WriteAsJsonAsync("Missing Rappi-Signature");
            throw new();
        }

        Dictionary<string, string> timestampAndSign = GetTimestampAndSign(signatureHeader.ToString());

        request.EnableBuffering();
        string body;
        using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true)) {
            body = await reader.ReadToEndAsync();
        }
        request.Body.Position = 0;

        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));

        var bodyDeserialized = JsonSerializer.Deserialize<object>(body);
        body = JsonSerializer.Serialize(bodyDeserialized);

        body = $"{timestampAndSign["t"]}.{body}";

        string signature = timestampAndSign["sign"];

        string secret = AppEnv.INTEGRATIONS.RAPPI.CLIENT.SECRET.NotNull();

        if (!signature.IsSignatureValid(secret, body)) {
            context.Response.StatusCode = 403;
            // logger.LogError("Signature Header invalid\n\nSignature sended: {signatureS}\n\nSignature Expected: {signatureE}", timestampAndSign["sign"], RappiSignatureValidator.GetExpectedSignature(secret, body));
            await context.Response.WriteAsJsonAsync("Invalid Signature");
        }

        return JsonSerializer.Deserialize<TRequest>(body)!;
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
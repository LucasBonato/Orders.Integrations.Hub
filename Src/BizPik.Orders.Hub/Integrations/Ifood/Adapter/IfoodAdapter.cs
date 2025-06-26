using System.Text;

using BizPik.Orders.Hub.Core.Application.Extensions;
using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Integrations.Common.Validators;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.Results;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BizPik.Orders.Hub.Integrations.Ifood.Adapter;

public abstract class IfoodAdapterLog;

public static class IfoodAdapter
{
    public static async Task<IResult> Webhook(
        ILogger<IfoodAdapterLog> logger,
        [FromServices] IOrderCreateUseCase<IfoodWebhookRequest> orderCreate,
        [FromServices] IOrderUpdateUseCase<IfoodWebhookRequest> orderUpdate,
        [FromServices] IOrderDisputeUseCase<IfoodWebhookRequest> orderDispute,
        HttpContext context
    ) {
        IfoodWebhookRequest request = await HandleSignature(context, logger);

        logger.LogInformation("[INFO] - IfoodAdapter - Ifood Webhook code: {FullCode}", request.FullCode);

        return request.FullCode switch
        {
            IfoodFullOrderStatus.KEEPALIVE => Accepted(),

            IfoodFullOrderStatus.PLACED => Accepted(value: await orderCreate.ExecuteAsync(request)),

            IfoodFullOrderStatus.CONFIRMED or
            IfoodFullOrderStatus.SEPARATION_STARTED or
            IfoodFullOrderStatus.SEPARATION_ENDED or
            IfoodFullOrderStatus.READY_TO_PICKUP or
            IfoodFullOrderStatus.DISPATCHED or
            IfoodFullOrderStatus.CONCLUDED or
            IfoodFullOrderStatus.CANCELLED => Accepted(value: await orderUpdate.ExecuteAsync(request)),

            IfoodFullOrderStatus.HANDSHAKE_DISPUTE or
            IfoodFullOrderStatus.HANDSHAKE_SETTLEMENT => Accepted(value: await orderDispute.ExecuteAsync(request)),

            _ => BadRequest(new { error = $"not mapped but ok {request.FullCode}" })
        };
    }

    private static async Task<IfoodWebhookRequest> HandleSignature(HttpContext context, ILogger<IfoodAdapterLog> logger)
    {
        string body;
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
        }
        //TODO: REMOVE THIS
        return JsonSerializer.Deserialize<IfoodWebhookRequest>(body)!;

        logger.LogInformation("[INFO] - IfoodSignatureValidator - Request Body: {body}", body);

        string? signature = context.Request.Headers["X-Ifood-Signature"].FirstOrDefault();
        string secret = AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNullEnv();

        if (signature == null)
        {
            logger.LogWarning("[WARN] - Signature header is missing.");
            throw new("Signature header is missing.");
        }

        if (!signature.IsSignatureValid(secret, body))
        {
            logger.LogWarning("[WARN] - Invalid signature.");
            throw new("Invalid signature");
        }

        return JsonSerializer.Deserialize<IfoodWebhookRequest>(body)!;
    }
}
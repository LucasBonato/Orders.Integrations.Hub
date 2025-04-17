using System.Security.Cryptography;
using System.Text;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.Results;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood;

public abstract class IfoodAdapterLog;

public static class IfoodAdapter
{
    public static async Task<IResult> Webhook(
        ILogger<IfoodAdapterLog> logger,
        [FromServices] ICreateOrderUseCase<IfoodWebhookRequest> createOrder,
        [FromServices] IUpdateOrderStatusUseCase<IfoodWebhookRequest> updateOrder,
        HttpContext context
    ) {
        IfoodWebhookRequest request = await HandleSignature(context, logger);

        logger.LogInformation("[INFO] - IfoodAdapter - Ifood Webhook code: {FullCode}", request.FullCode);

        return request.FullCode switch
        {
            IfoodFullOrderStatus.KEEPALIVE => Accepted(),

            IfoodFullOrderStatus.PLACED => Accepted("/", await createOrder.ExecuteAsync(request)),

            IfoodFullOrderStatus.CONFIRMED or
            IfoodFullOrderStatus.SEPARATION_STARTED or
            IfoodFullOrderStatus.SEPARATION_ENDED or
            IfoodFullOrderStatus.READY_TO_PICKUP or
            IfoodFullOrderStatus.DISPATCHED or
            IfoodFullOrderStatus.CONCLUDED or
            IfoodFullOrderStatus.CANCELLED => Accepted( "/", await updateOrder.ExecuteAsync(request)),

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

        logger.LogInformation("[INFO] - IfoodSignatureValidator - Request Body: {body}", body);

        string? signature = context.Request.Headers["X-Ifood-Signature"].FirstOrDefault();

        if (signature == null)
        {
            throw new("Signature header is missing.");
        }

        if (!IsSignatureValid(signature, body, logger))
        {
            throw new("Invalid signature");
        }

        return JsonSerializer.Deserialize<IfoodWebhookRequest>(body)!;
    }

    private static bool IsSignatureValid(string headerSignature, string body, ILogger<IfoodAdapterLog> logger)
    {
        logger.LogInformation("[INFO] - IfoodSignatureValidator - Expected [X-Ifood-Signature] Signature: [{signature}]", headerSignature);

        string generatedSignature = GetExpectedSignature(AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNull(), body);

        logger.LogInformation("[INFO] - IfoodSignatureValidator - Generated Signature: [{signature}]", generatedSignature);

        return generatedSignature == headerSignature;
    }

    private static string GetExpectedSignature(string secret, string data)
    {
        using HMACSHA256 hmacSha256 = new (Encoding.UTF8.GetBytes(secret));
        byte[] hmacBytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        string hmacHex = Convert.ToHexStringLower(hmacBytes);
        return hmacHex;
    }
}
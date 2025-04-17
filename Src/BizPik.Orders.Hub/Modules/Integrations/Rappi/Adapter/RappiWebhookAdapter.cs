using System.Text;
using System.Text.Json;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Validators;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Adapter;

public abstract class RappiWebhookAdapterLog;

public static class RappiWebhookAdapter {
    public static async Task<IResult> CreateOrder(
        [FromServices] ICreateOrderUseCase<RappiOrder> createOrder,
        [FromServices] IUpdateOrderStatusUseCase<RappiOrder> updateOrder,
        ILogger<RappiWebhookAdapterLog> logger,
        HttpContext context
    ) {
        RappiOrder request = await HandleSignature(context, logger);
        return Created();
    }

    public static async Task<IResult> AutoAcceptOrder(HttpContext context)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> CancelOrder(HttpContext context)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> PatchOrder(HttpContext context)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> PingStore(HttpContext context)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// É chamado toda vez que uma requisição é realizada, pegando o corpo da requisição <br/>
    /// e tratando para verificar se o header `Rappi-Signature` é válido e foi enviado pela Rappi.
    /// </summary>
    /// <param name="context">Informações da requição http</param>
    /// <returns></returns>
    private static async Task<RappiOrder> HandleSignature(HttpContext context, ILogger<RappiWebhookAdapterLog> logger) {
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

        string secret = string.Empty;
        // string secret = AppEnv.INTEGRATIONS.RAPPI.CLIENT.SECRET.NotNull();

        if (!RappiSignatureValidator.VerifyHmacSHA256(secret, body, timestampAndSign["sign"])) {
            context.Response.StatusCode = 403;
            logger.LogError("Signature Header invalid\n\nSignature sended: {signatureS}\n\nSignature Expected: {signatureE}", timestampAndSign["sign"], RappiSignatureValidator.GetExpectedSignature(secret, body));
            await context.Response.WriteAsJsonAsync("Invalid Signature");
        }

        return JsonSerializer.Deserialize<RappiOrder>(body)!;
    }

    /// <summary>
    /// Divide o header `Rappi-Signature`, pegando o timestamp ('t') e a signature ('sign') separadamente.
    /// </summary>
    /// <param name="header">Header `Rappi-Signature` recebido na requisição.</param>
    /// <returns>Retorna um Dictionary com os valores do timestamp e signature, sendo chamada com as seguintes keys: ["t"] e ["sign"]</returns>
    /// <exception cref="ArgumentException"></exception>
    private static Dictionary<string, string> GetTimestampAndSign(string header) {
        Dictionary<string, string> response = new();
        string[] timestampAndSignature = header.Split(",");
        foreach (string item in timestampAndSignature) {

            string[] timestampOrSignatureDivided = item.Split("=");

            if (timestampOrSignatureDivided[0] == "t" || timestampOrSignatureDivided[0] == "sign") {
                response.Add(timestampOrSignatureDivided[0], timestampOrSignatureDivided[1]);
            }
        }
        if (response.Count < 1)
            throw new ArgumentException("O Header não possui 't' e 'sign'.");
        return response;
    }
}
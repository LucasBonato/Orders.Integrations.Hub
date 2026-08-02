using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Core.Adapters.In.Http;

internal sealed class OrdersHubProductStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app
                .MapGroup("/orders/products")
                .ExcludeFromDescription()
            ;
        
        group.MapPost("/enable", async (
            [FromServices] IIntegrationRouter router,
            HttpRequest request
        ) => {
            object body = await ReadBodyFromSimpleNotificationService<object>(request);
            await router.Resolve<IOrderChangeProductStatusUseCase>(IntegrationKey.Nothing()).Enable(body);
            return NoContent();
        });
        
        group.MapPost("/disable", async (
            [FromServices] IIntegrationRouter router,
            HttpRequest request
        ) => {
            object body = await ReadBodyFromSimpleNotificationService<object>(request);
            await router.Resolve<IOrderChangeProductStatusUseCase>(IntegrationKey.Nothing()).Disable(body);
            return NoContent();
        });
    }
    
    private static async Task<T> ReadBodyFromSimpleNotificationService<T>(HttpRequest request) {
        using StreamReader reader = new (request.Body, Encoding.UTF8);
        string body = await reader.ReadToEndAsync();

        SnsWrapper snsMessage = JsonSerializer.Deserialize<SnsWrapper>(body)!;

        T response = JsonSerializer.Deserialize<T>(snsMessage.Message)!;

        return response;
    }
    
    record SnsWrapper(string Message);
}
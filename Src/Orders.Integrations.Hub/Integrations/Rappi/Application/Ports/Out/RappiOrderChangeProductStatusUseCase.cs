using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.Out;

public class RappiOrderChangeProductStatusUseCase(
    IRappiClient rappiClient
) : IOrderChangeProductStatusUseCase {
    public Task Enable(object productEvent)
    {
        object[] stores = [];
        string storeId = string.Empty;
        List<string> turnOn = [];

        if (stores.Length is 0)
        {
            throw new Exception();
        }

        Task[] requests = stores
            .Select(store => new RappiAvailabilityUpdateItemsRequest(
                StoreIntegrationId: storeId,
                Items: new RappiAvailabilityItem(
                    TurnOn: turnOn,
                    TurnOff: []
                )
            ))
            .Select((Func<RappiAvailabilityUpdateItemsRequest, Task>)MakeUpdateProductRequest)
            .ToArray();

        Task.WaitAll(requests);

        return Task.CompletedTask;
    }

    public Task Disable(object productEvent)
    {
        object[] stores = [];
        string storeId = string.Empty;
        List<string> turnOff = [];

        if (stores.Length is 0)
        {
            throw new Exception();
        }

        Task[] requests = stores
            .Select(store => new RappiAvailabilityUpdateItemsRequest(
                StoreIntegrationId: storeId,
                Items: new RappiAvailabilityItem(
                    TurnOn: [],
                    TurnOff: turnOff
                )
            ))
            .Select((Func<RappiAvailabilityUpdateItemsRequest, Task>)MakeUpdateProductRequest)
            .ToArray();

        Task.WaitAll(requests);

        return Task.CompletedTask;
    }

    private async Task MakeUpdateProductRequest(RappiAvailabilityUpdateItemsRequest request)
    {
        await rappiClient.PutAvailabilityProductsStatus([request]);
    }
}
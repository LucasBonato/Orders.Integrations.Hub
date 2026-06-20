using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.Out;

public class RappiOrderChangeProductStatusUseCase(
    IRappiClient rappiClient
) : IOrderChangeProductStatusUseCase {
    public Task Enable(object productEvent)
    {
        return TurnProductsOnOrOff(
            turnOn: [],    
            turnOff: []
        );
    }

    public Task Disable(object productEvent)
    {
        return TurnProductsOnOrOff(
            turnOn: [],    
            turnOff: []
        );
    }

    private Task TurnProductsOnOrOff(List<string> turnOn, List<string> turnOff)
    {
        object[] stores = [];
        string storeId = string.Empty;

        if (stores.Length is 0)
            throw new Exception();

        Task[] requests = stores
            .Select(_ => new RappiAvailabilityUpdateItemsRequest(
                StoreIntegrationId: storeId,
                Items: new RappiAvailabilityItem(
                    TurnOn: [],
                    TurnOff: turnOff
                )
            ))
            .Select(MakeUpdateProductRequest)
            .ToArray();

        Task.WaitAll(requests);

        return Task.CompletedTask;
    }

    private async Task MakeUpdateProductRequest(RappiAvailabilityUpdateItemsRequest request)
    {
        await rappiClient.PutAvailabilityProductsStatus([request]);
    }
}

using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports;

public class RappiOrderChangeProductStatusUseCase(
    IRappiClient rappiClient,
    IInternalClient Client
) : IOrderChangeProductStatusUseCase {
    public OrderIntegration Integration => OrderIntegration.RAPPI;
    
    public async Task Enable(SNSProductEvent productEvent)
    {
        ResponseWrapper<List<IntegrationResponse>> response = await Client.GetIntegrationByCompanyId(productEvent.CompanyId.ToString());
        IEnumerable<IntegrationResponse> integrations = response.Data.Where(integration => integration.IntegrationId is 26);
        RappiIntegrationResolved[] stores = integrations.Select(integration => integration.Resolve()).ToArray();

        if (stores.Length is 0)
        {
            throw new();
        }

        Task[] requests = stores
            .Select(store => new RappiAvailabilityUpdateItemsRequest(
                StoreIntegrationId: store.RappiStoreId,
                Items: new RappiAvailabilityItem(
                    TurnOn: productEvent.ProductSkus,
                    TurnOff: []
                )
            ))
            .Select((Func<RappiAvailabilityUpdateItemsRequest, Task>)MakeUpdateProductRequest)
            .ToArray();

        Task.WaitAll(requests);
        return;

        async Task MakeUpdateProductRequest(RappiAvailabilityUpdateItemsRequest request)
        {
            await rappiClient.PutAvailabilityProductsStatus([request]);
        }
    }

    public async Task Disable(SNSProductEvent productEvent)
    {
        ResponseWrapper<List<IntegrationResponse>> response = await Client.GetIntegrationByCompanyId(productEvent.CompanyId.ToString());
        IEnumerable<IntegrationResponse> integrations = response.Data.Where(integration => integration.IntegrationId is 26);
        RappiIntegrationResolved[] stores = integrations.Select(integration => integration.Resolve()).ToArray();

        if (stores.Length is 0)
        {
            throw new();
        }

        Task[] requests = stores
            .Select(store => new RappiAvailabilityUpdateItemsRequest(
                StoreIntegrationId: store.RappiStoreId,
                Items: new RappiAvailabilityItem(
                    TurnOn: [],
                    TurnOff: productEvent.ProductSkus
                )
            ))
            .Select((Func<RappiAvailabilityUpdateItemsRequest, Task>)MakeUpdateProductRequest)
            .ToArray();

        Task.WaitAll(requests);
        return;

        async Task MakeUpdateProductRequest(RappiAvailabilityUpdateItemsRequest request)
        {
            await rappiClient.PutAvailabilityProductsStatus([request]);
        }
    }
}
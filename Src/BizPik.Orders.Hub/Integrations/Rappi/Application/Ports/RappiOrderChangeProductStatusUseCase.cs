using BizPik.Orders.Hub.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.BizPik;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Integrations.Rappi.Application.Ports;

public class RappiOrderChangeProductStatusUseCase(
    IRappiClient rappiClient,
    IBizPikMonolithClient bizPikClient
) : IOrderChangeProductStatusUseCase {
    public OrderIntegration Integration => OrderIntegration.RAPPI;
    
    public async Task Enable(BizPikSNSProductEvent productEvent)
    {
        BizPikResponseWrapper<List<BizPikIntegrationResponse>> response = await bizPikClient.GetIntegrationByCompanyId(productEvent.CompanyId.ToString());
        IEnumerable<BizPikIntegrationResponse> integrations = response.Data.Where(integration => integration.IntegrationId is 26);
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

    public async Task Disable(BizPikSNSProductEvent productEvent)
    {
        BizPikResponseWrapper<List<BizPikIntegrationResponse>> response = await bizPikClient.GetIntegrationByCompanyId(productEvent.CompanyId.ToString());
        IEnumerable<BizPikIntegrationResponse> integrations = response.Data.Where(integration => integration.IntegrationId is 26);
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
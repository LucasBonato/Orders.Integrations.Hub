using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderChangeProductStatusUseCase(
    IIFoodClient ifoodClient
) : IOrderChangeProductStatusUseCase {
    public OrderIntegration Integration => OrderIntegration.IFOOD;

    public async Task Enable(BizPikSNSProductEvent productEvent)
    {
        foreach (var payload in productEvent.ProductSkus.Select(sku => IfoodPatchProductStatusRequest.Enable(itemId: sku, statusByCatalog: []))) {
            // Don't throw on failed requests
            await ifoodClient.PatchProductStatus(
                productEvent.MerchantId,
                payload
            );
        }
    }

    public async Task Disable(BizPikSNSProductEvent productEvent)
    {
        foreach (var payload in productEvent.ProductSkus.Select(sku => IfoodPatchProductStatusRequest.Disable(itemId: sku, statusByCatalog: []))) {
            // Don't throw on failed requests
            await ifoodClient.PatchProductStatus(
                productEvent.MerchantId,
                payload
            );
        }
    }
}
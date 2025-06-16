using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Ports.Out;

public class IfoodOrderChangeProductStatusUseCase(
    IIFoodClient ifoodClient
) : IOrderChangeProductStatusUseCase {
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
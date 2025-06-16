using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports;

public class IfoodOrderChangeProductStatusUseCase(
    IIFoodClient ifoodClient
) : IOrderChangeProductStatusUseCase {
    public async Task Enable(SNSProductEvent productEvent)
    {
        foreach (var payload in productEvent.ProductSkus.Select(sku => IfoodPatchProductStatusRequest.Enable(itemId: sku, statusByCatalog: []))) {
            // Don't throw on failed requests
            await ifoodClient.PatchProductStatus(
                productEvent.MerchantId,
                payload
            );
        }
    }

    public async Task Disable(SNSProductEvent productEvent)
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
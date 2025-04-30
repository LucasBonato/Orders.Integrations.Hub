using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderChangeProductStatusUseCase(
    IIFoodClient ifoodClient
) : IOrderChangeProductStatusUseCase {
    public OrderIntegration Integration => OrderIntegration.IFOOD;

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
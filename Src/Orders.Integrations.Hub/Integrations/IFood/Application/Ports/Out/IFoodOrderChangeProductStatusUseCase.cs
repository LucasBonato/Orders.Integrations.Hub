using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.Out;

public class IFoodOrderChangeProductStatusUseCase(
    IIFoodClient ifoodClient
) : IOrderChangeProductStatusUseCase {
    public async Task Enable(object productEvent)
    {
        IEnumerable<string> skus = [];
        string merchantId = string.Empty;
        foreach (var payload in skus.Select(sku => IFoodPatchProductStatusRequest.Enable(itemId: sku, statusByCatalog: []))) {
            // Don't throw on failed requests
            await ifoodClient.PatchProductStatus(
                merchantId,
                payload
            );
        }
    }

    public async Task Disable(object productEvent)
    {
        IEnumerable<string> skus = [];
        string merchantId = string.Empty;
        foreach (var payload in skus.Select(sku => IFoodPatchProductStatusRequest.Disable(itemId: sku, statusByCatalog: []))) {
            // Don't throw on failed requests
            await ifoodClient.PatchProductStatus(
                merchantId,
                payload
            );
        }
    }
}
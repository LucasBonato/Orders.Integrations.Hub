using System.Text.Json;

using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Handshake;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodHandshakeOrderDisputeUseCase : IOrderDisputeUseCase<IfoodWebhookRequest>
{
    public async Task ExecuteAsync(string orderId, IfoodWebhookRequest ifoodOrder)
    {
        string json = JsonSerializer.Serialize(ifoodOrder.Metadata);

        HandshakeDispute dispute = JsonSerializer.Deserialize<HandshakeDispute>(json)
                                   ?? throw new Exception("Não foi possível converter disputa!");




        throw new NotImplementedException();
    }
}
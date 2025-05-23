using System.Text.Json;

using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Ports;

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
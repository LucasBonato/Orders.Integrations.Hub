using System.Text.Json;

using BizPik.Orders.Hub.Core.Domain.Contracts;
using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Integrations.Common;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Clients;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Ports.In;

public class IfoodHandshakeOrderDisputeUseCase(
    IObjectStorageClient objectStorageClient,
    IfoodClient ifoodClient
) : IOrderDisputeUseCase<IfoodWebhookRequest> {
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest ifoodOrder)
    {
        OrderEventType type = ifoodOrder.FullCode is IfoodFullOrderStatus.HANDSHAKE_DISPUTE
            ? OrderEventType.DISPUTE_STARTED
            : OrderEventType.DISPUTE_FINISH;

        HandshakeDispute? dispute = null;

        string json = JsonSerializer.Serialize(ifoodOrder.Metadata);

        if (type == OrderEventType.DISPUTE_STARTED) {
            dispute = JsonSerializer.Deserialize<HandshakeDispute>(json);

            if (dispute?.Metadata?.Evidences is not null) {
                dispute.Metadata.Evidences = await UploadEvidencesToObjectStorage(ifoodOrder.OrderId, dispute);
            }
        }
        else {
            HandshakeSettlement? settlement = JsonSerializer.Deserialize<HandshakeSettlement>(json);
            if (settlement != null)
            {
                dispute = new HandshakeDispute(
                    DisputeId: settlement.DisputeId,
                    ParentDisputeId: settlement.DisputeId,
                    Message: settlement.Reason?? settlement.Status.ToString(),
                    Alternatives: settlement.SelectedDisputeAlternative is { } alternative
                        ? [
                            new DisputeAlternative(
                                Id: alternative.Id,
                                Type: alternative.Type,
                                Metadata: alternative.Metadata,
                                Amount: alternative.Metadata.Amount,
                                AllowedsAdditionalTimeInMinutes: alternative.Metadata.AllowedsAdditionalTimeInMinutes,
                                AllowedsAdditionalTimeReasons: alternative.Metadata.AllowedsAdditionalTimeReasons
                            )
                        ]
                        : null,
                    Action: default,
                    TimeoutAction: default,
                    ExpiresAt: default,
                    CreatedAt: settlement.CreatedAt,
                    HandshakeType: default,
                    HandshakeGroup: default,
                    Metadata: null
                );

                await objectStorageClient.DeleteFolder(GenerateKeyUrlPath(ifoodOrder.OrderId, dispute.DisputeId));
            }
        }

        if (dispute is null)
            throw new Exception("Não foi possível converter disputa!");

        await new ProcessOrderDisputeEvent(
            ExternalOrderId: ifoodOrder.OrderId,
            Integration: OrderIntegration.IFOOD,
            OrderDispute: dispute.ToOrder(),
            Type: type
        ).PublishAsync();

        return ifoodOrder;
    }

    private async Task<List<Media>> UploadEvidencesToObjectStorage(string orderId, HandshakeDispute dispute) {
        List<Task<Media>>? tasks = dispute.Metadata?.Evidences?.Select(
            async evidence => {
                string imageId = evidence.Url.Split('/').Last();
                DownloadFile file = await ifoodClient.GetDisputeImage(evidence.Url);

                string keyPath = GenerateKeyUrlFile(orderId, dispute.DisputeId, imageId);

                await using Stream fileStream = new MemoryStream(file.Bytes);
                string fileUrl = await objectStorageClient.UploadFile(fileStream, file.ContentType, keyPath);

                return new Media(
                    Url: fileUrl,
                    ContentType: file.ContentType
                );
            }
        ).ToList();

        return tasks is null ? [] : (await Task.WhenAll(tasks)).ToList();
    }

    private static string GenerateKeyUrlPath(string orderId, string disputeId) {
        return $"dispute/{orderId}/{disputeId}";
    }

    private static string GenerateKeyUrlFile(string orderId, string disputeId, string fileName)
    {
        return $"{GenerateKeyUrlPath(orderId, disputeId)}/{Guid.CreateVersion7()}_{fileName}";
    }
}
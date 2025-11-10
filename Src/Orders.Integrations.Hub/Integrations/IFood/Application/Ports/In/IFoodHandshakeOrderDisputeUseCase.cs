using System.Text.Json;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;

public class IFoodHandshakeOrderDisputeUseCase(
    IObjectStorageClient objectStorageClient,
    IIFoodClient ifoodClient
) : IOrderDisputeUseCase<IFoodWebhookRequest> {
    public async Task<IFoodWebhookRequest> ExecuteAsync(IFoodWebhookRequest foodOrder)
    {
        OrderEventType type = foodOrder.FullCode is IFoodFullOrderStatus.HANDSHAKE_DISPUTE
            ? OrderEventType.DISPUTE_STARTED
            : OrderEventType.DISPUTE_FINISH;

        HandshakeDispute? dispute = null;

        string json = JsonSerializer.Serialize(foodOrder.Metadata);

        if (type == OrderEventType.DISPUTE_STARTED) {
            dispute = JsonSerializer.Deserialize<HandshakeDispute>(json);

            if (dispute?.Metadata?.Evidences is not null) {
                dispute.Metadata.Evidences = await UploadEvidencesToObjectStorage(foodOrder.OrderId, dispute);
            }
        }
        else {
            HandshakeSettlement? settlement = JsonSerializer.Deserialize<HandshakeSettlement>(json);
            if (settlement != null)
            {
                dispute = new HandshakeDispute(
                    DisputeId: settlement.DisputeId,
                    ParentDisputeId: settlement.DisputeId,
                    Message: settlement.Status.ToString(), //?? settlement.Reason?? string.Empty,
                    Alternatives: settlement.SelectedDisputeAlternative is { } alternative
                        ? [
                            new IFoodDisputeAlternative(
                                Id: alternative.Id,
                                Type: alternative.Type,
                                Metadata: alternative.Metadata,
                                MaxAmount: alternative.Metadata.MaxAmount,
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

                await objectStorageClient.DeleteFolder(GenerateKeyUrlPath(foodOrder.OrderId, dispute.DisputeId));
            }
        }

        if (dispute is null)
            throw new Exception("Não foi possível converter disputa!");

        await new ProcessOrderDisputeEvent(
            ExternalOrderId: foodOrder.OrderId,
            Integration: OrderIntegration.IFOOD,
            OrderDispute: dispute.ToOrder(),
            Type: type
        ).PublishAsync();

        return foodOrder;
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
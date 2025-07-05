using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

using Dispute_DisputeAlternative = Orders.Integrations.Hub.Core.Domain.Entity.Dispute.DisputeAlternative;
using DisputeAlternative = Orders.Integrations.Hub.Core.Domain.Entity.Dispute.DisputeAlternative;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Extensions;

public static class IfoodOrderDisputeExtension
{
    public static OrderDispute? ToOrder(this HandshakeDispute? ifoodDispute)
    {
        if (ifoodDispute == null)
            return null;

        List<Dispute_DisputeAlternative>? alternatives = ifoodDispute.Alternatives?
            .Select(alternative =>
                new Dispute_DisputeAlternative(
                   AlternativeId: alternative.Id,
                   Type: alternative.Type,
                   Price: (alternative.Metadata is null) ? alternative.Amount?.ToPrice() : alternative.Metadata.Amount?.ToPrice(),
                   AllowedTimesInMinutes: (alternative.Metadata is null) ? alternative.AllowedsAdditionalTimeInMinutes : alternative.Metadata.AllowedsAdditionalTimeInMinutes,
                   AllowedTimesReasons: (alternative.Metadata is null) ? alternative.AllowedsAdditionalTimeReasons : alternative.Metadata.AllowedsAdditionalTimeReasons?
                       .Select(reason => reason.ToString())
                       .ToList()?? []
                )
            ).ToList();

        List<DisputeEvidence>? evidences = ifoodDispute.Metadata?.Evidences?
            .Select(evidence =>
                new DisputeEvidence(
                    Url: evidence.Url
                )
            ).ToList();

        List<DisputeItem>? items = ifoodDispute.Metadata?.Items?
            .Select(item =>
                new DisputeItem(
                    ExternalId: item.Id,
                    ExternalUniqueId: item.UniqueId,
                    Sku: item.ExternalCode?? item.IntegrationId?? string.Empty,
                    Index: item.Index,
                    Quantity: item.Quantity,
                    Price: item.Amount.ToPrice(),
                    ReasonMessage: item.Reason?? string.Empty
                )
            ).ToList();

        List<DisputeItemOption>? options = ifoodDispute.Metadata?.GarnishItems?
            .Select(option =>
                new DisputeItemOption(
                    ExternalId: option.Id,
                    ParentExternalUniqueId: option.ParentUniqueId,
                    Sku: option.ExternalCode,
                    Index: option.Index,
                    Quantity: option.Quantity,
                    Price: option.Amount.ToPrice(),
                    ReasonMessage: option.Reason?? string.Empty
                )
            ).ToList();

        List<string>? cancellationReasons = ifoodDispute.Metadata?.AcceptCancellationReasons?
            .Select(reason => reason.ToString())
            .ToList();

        return new OrderDispute(
            DisputeId: ifoodDispute.DisputeId,
            Message: ifoodDispute.Message?? string.Empty,
            CreatedAt: ifoodDispute.CreatedAt,
            ExpiresAt: ifoodDispute.ExpiresAt,
            Action: ifoodDispute.Action.ToString(),
            TimeoutAction: ifoodDispute.TimeoutAction.ToString(),
            Alternatives: alternatives,
            Evidences: evidences,
            Items: items,
            Options: options,
            CancellationReasons: cancellationReasons
        );
    }

    private static Price ToPrice(this Amount amount)
    {
        // Ifood send the value as string without the `,` or `.`, like `1500` is 15.00
        decimal valueConverted = Convert.ToDecimal(amount.Value) / 100;

        return new Price(
            Value: valueConverted,
            Currency: amount.Currency
        );
    }
}
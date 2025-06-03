using BizPik.Orders.Hub.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

using DisputeAlternative = BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute.DisputeAlternative;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Extensions;

public static class IfoodOrderDisputeExtension
{
    public static OrderDispute? ToOrder(this HandshakeDispute? ifoodDispute)
    {
        if (ifoodDispute == null)
            return null;

        List<DisputeAlternative>? alternatives = ifoodDispute.Alternatives?
            .Select(alternative =>
                new DisputeAlternative(
                   AlternativeId: alternative.Id,
                   Type: alternative.Type,
                   Price: alternative.Metadata.Amount?.ToPrice(),
                   AllowedTimesInMinutes: alternative.Metadata.AllowedsAdditionalTimeInMinutes,
                   AllowedTimesReasons: alternative.Metadata.AllowedsAdditionalTimeReasons?
                       .Select(reason => reason.ToString())
                       .ToList()
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
                    Sku: item.ExternalCode,
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
            DisputeId: ifoodDispute.Id,
            Message: ifoodDispute.Message,
            CreatedAt: ifoodDispute.CreatedAt,
            ExpiresAt: ifoodDispute.ExpiresAt,
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
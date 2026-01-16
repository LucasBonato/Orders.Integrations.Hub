using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;

public static class IFoodOrderDisputeExtension
{
    public static OrderDispute? ToOrder(this HandshakeDispute? ifoodDispute)
    {
        if (ifoodDispute == null)
            return null;

        List<DisputeAlternative>? alternatives = ifoodDispute.Alternatives?
            .Select(alternative =>
                new DisputeAlternative(
                   AlternativeId: alternative.Id,
                   Type: alternative.Type.ToOrder(),
                   Price: (alternative.Metadata is null) ? alternative.MaxAmount?.ToPrice() : alternative.Metadata.MaxAmount?.ToPrice(),
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

    private static AlternativeType ToOrder(this HandshakeAlternativeType type)
    {
        return type switch {
            HandshakeAlternativeType.REFUND => AlternativeType.REFUND,
            HandshakeAlternativeType.BENEFIT => AlternativeType.BENEFIT,
            HandshakeAlternativeType.ADDITIONAL_TIME => AlternativeType.ADDITIONAL_TIME,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static Price ToPrice(this Amount amount)
    {
        // IFood send the value as string without the `,` or `.`, like `1500` is 15.00
        decimal valueConverted = Convert.ToDecimal(amount.Value) / 100;

        return new Price(
            Value: valueConverted,
            Currency: amount.Currency
        );
    }
}
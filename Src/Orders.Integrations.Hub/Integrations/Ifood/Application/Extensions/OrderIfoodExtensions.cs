using System.Globalization;

using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Extensions;

public static class OrderIfoodExtensions
{
    public static Amount ToAmount(this Price price)
    {
        string value = (price.Value * 100).ToString("F0", CultureInfo.InvariantCulture);

        return new Amount(
            Value: value,
            Currency: price.Currency
        );
    }
}
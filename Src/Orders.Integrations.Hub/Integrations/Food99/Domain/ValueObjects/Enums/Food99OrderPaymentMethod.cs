namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

public enum Food99OrderPaymentMethod
{
    OnlinePayment = 1,
    Cash = 2,
    POSDelivery = 3,
    Wallet99Food = 4,
    PayPalWithoutSecret = 5,
    PayPalWithSecret = 6
}
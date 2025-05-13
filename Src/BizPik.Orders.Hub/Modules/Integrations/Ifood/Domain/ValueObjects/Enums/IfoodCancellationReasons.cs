namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

public enum IfoodCancellationReasons
{
    /// <summary>
    /// Problemas de sistema na loja
    /// </summary>
    MERCHANT_SYSTEM_PROBLEMS = 501,

    /// <summary>
    /// O pedido está duplicado
    /// </summary>
    ORDER_DUPLICATE = 502,

    /// <summary>
    /// Item indisponível
    /// </summary>
    ITEM_UNAVAILABLE = 503,

    /// <summary>
    /// A loja está sem entregadores disponíveis
    /// </summary>
    MERCHANT_WITHOUT_DRIVER = 504,

    /// <summary>
    /// O cardápio está desatualizado
    /// </summary>
    MENU_OUTDATED = 505,

    /// <summary>
    /// O pedido está fora da área de entrega
    /// </summary>
    ORDER_OUT_OF_DELIVERY_ZONE = 506,

    /// <summary>
    /// Suspeita de golpe ou trote
    /// </summary>
    CUSTOMER_FRAUD = 507,

    /// <summary>
    /// O pedido foi feito fora do horário de funcionamento da loja
    /// </summary>
    ORDER_OUT_OF_DELIVERY_TIME = 508,

    /// <summary>
    /// A loja está passando por dificuldades internas
    /// </summary>
    MERCHANT_INTERNAL_PROBLEMS = 509,

    /// <summary>
    /// A entrega é em uma área de risco
    /// </summary>
    DELIVERY_RISK_ZONE = 511,

    /// <summary>
    /// A loja só abrirá mais tardeÄ
    /// </summary>
    MERCHANT_OPEN_LATER = 512,

    /// <summary>
    /// O endereço está incompleto e o cliente não atende.
    /// </summary>
    CUSTOMER_DONT_ANSWER_AND_INCOMPLETE_ADDRESS = 520,

    /// <summary>
    /// Problema com pagamento do cliente
    /// </summary>
    CUSTOMER_PAYMENT_PROBLEM = 860,
}
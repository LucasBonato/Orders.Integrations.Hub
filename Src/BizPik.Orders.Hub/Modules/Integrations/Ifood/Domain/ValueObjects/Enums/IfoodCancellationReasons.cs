namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

public enum IfoodCancellationReasons
{
    /// <summary>
    /// Problemas de sistema na loja
    /// </summary>
    SystemProblems = 501,

    /// <summary>
    /// O pedido está duplicado
    /// </summary>
    OrderDuplicate = 502,

    /// <summary>
    /// Item indisponível
    /// </summary>
    ItemUnavailable = 503,

    /// <summary>
    /// A loja está sem entregadores disponíveis
    /// </summary>
    MerchantWithoutDeliveryMan = 504,

    /// <summary>
    /// O cardápio está desatualizado
    /// </summary>
    MenuOutdated = 505,

    /// <summary>
    /// O pedido está fora da área de entrega
    /// </summary>
    OrderOutOfDeliveryZone = 506,

    /// <summary>
    /// Suspeita de golpe ou trote
    /// </summary>
    Fraude = 507,

    /// <summary>
    /// O pedido foi feito fora do horário de funcionamento da loja
    /// </summary>
    OutOfDeliveryTime = 508,

    /// <summary>
    /// A loja está passando por dificuldades internas
    /// </summary>
    MerchantInternalProblems = 509,

    /// <summary>
    /// A entrega é em uma área de risco
    /// </summary>
    RiskZone = 511,

    /// <summary>
    /// A loja só abrirá mais tardeÄ
    /// </summary>
    MerchantOpenLater = 512
}
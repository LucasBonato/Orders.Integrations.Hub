namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

public enum IfoodCancellationReasons
{
    /// <summary>
    /// PROBLEMAS DE SISTEMA
    /// </summary>
    SystemProblems = 501,

    /// <summary>
    /// PEDIDO EM DUPLICIDADE
    /// </summary>
    OrderDuplicate = 502,

    /// <summary>
    /// ITEM INDISPONÍVEL
    /// </summary>
    ItemUnavailable = 503,

    /// <summary>
    /// RESTAURANTE SEM MOTOBOY
    /// </summary>
    MerchantWithoutDeliveryMan = 504,

    /// <summary>
    /// CARDÁPIO DESATUALIZADO
    /// </summary>
    MenuOutdated = 505,

    /// <summary>
    /// PEDIDO FORA DA ÁREA DE ENTREGA
    /// </summary>
    OrderOutOfDeliveryZone = 506,

    /// <summary>
    /// CLIENTE GOLPISTA / TROTE
    /// </summary>
    Fraude = 507,

    /// <summary>
    /// FORA DO HORÁRIO DO DELIVERY
    /// </summary>
    OutOfDeliveryTime = 508,

    /// <summary>
    /// DIFICULDADES INTERNAS DO RESTAURANTE
    /// </summary>
    MerchantInternalProblems = 509,

    /// <summary>
    /// ÁREA DE RISCO
    /// </summary>
    RiskZone = 511,

    /// <summary>
    /// RESTAURANTE ABRIRÁ MAIS TARDE
    /// </summary>
    MerchantOpenLater = 512
}
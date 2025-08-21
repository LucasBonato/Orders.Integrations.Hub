using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

public record Food99WebhookRequest(
    long AppId,
    string AppShopId,
    long Timestamp,
    Food99Type Type,
    Food99Data Data
);

public record Food99Data(
    long OrderId,
    Food99OrderInfo OrderInfo,
    Food99DeliveryStatus? DeliveryStatus,
    string? RiderName,
    string? RiderPhone,
    string? ApplyReason,
    long? ApplyId,
    List<Food99Reason>? ReasonList,
    List<string>? Images,
    List<Food99BaseReason>? BaseReasonList
);

public record Food99Reason(
    string Reason
);

public record Food99BaseReason(
    string BaseReasonId,
    string BaseReason
);

public record Food99OrderInfo(
    long OrderId,
    int Status,
    int OrderIndex,
    string Remark,
    long CityId,
    string Country,
    string Timezone,
    Food99OrderPaymentMethod PayType,
    int DeliveryType,
    long ExpectedCookEta,
    long ExpectedArrivedEta,
    long CreateTime,
    long PayTime,
    long CompleteTime,
    long CancelTime,
    long ShopConfirmTime,
    Food99Price Price,
    Food99Shop Shop,
    Food99ReceiveAddress ReceiveAddress,
    List<Food99Item> OrderItems,
    List<Food99Promotion>? Promotions
);

public record Food99Price(
    int OrderPrice,
    int? RealPrice,
    int? RealPayPrice,
    int DeliveryPrice,
    int RefundPrice,
    int ItemsDiscount,
    int DeliveryDiscount,
    Food99OtherFees OthersFees,
    int CustomerNeedPayingMoney
);

public record Food99OtherFees(
    int SmallOrderPrice,
    int TotalTipMoney,
    int ServicePrice,
    int CouponDiscount
);

public record Food99Shop(
    long ShopId,
    string AppShopId,
    string ShopAddr,
    string ShopName,
    List<Food99ShopPhone> ShopPhone
);

public record Food99ShopPhone(
    int CallingCode,
    long Phone,
    string Type
);

public record Food99ReceiveAddress(
    long Uid,
    string Name,
    string FirstName,
    string LastName,
    string CallingCode,
    string Phone,
    string City,
    string CountryCode,
    string PoiAddress,
    string HouseNumber,
    double PoiLat,
    double PoiLng,
    string CoordinateType,
    string? PoiDisplayName,
    string? State,
    string? District,
    string? StreetName,
    string StreetNumber,
    string? PostalCode,
    string? Complement,
    string? Reference
);


public record Food99Item(
    string AppItemId,
    string AppExternalId,
    string Name,
    int TotalPrice,
    int SkuPrice,
    int Amount,
    string Remark,
    List<Food99SubItem> SubItemList,
    int RealPrice,
    Food99OrderPromoType PromoType,
    Food99Promotion? PromotionDetail,
    List<Food99Promotion>? PromoList
);

public record Food99SubItem(
    string AppItemId,
    string AppExternalId,
    string Name,
    int TotalPrice,
    int SkuPrice,
    int Amount,
    string AppContentId,
    string ContentAppExternalId,
    List<Food99SubItem> SubItemList
);

public record Food99Promotion(
    Food99OrderPromoType PromoType,
    int PromoDiscount,
    int ShopSubsidePrice
);

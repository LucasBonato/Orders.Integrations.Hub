using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Extensions;

public static class IfoodOrderExtension
{
    public static Order ToOrder(this IfoodOrder order, int companyId)
    {
        throw new NotImplementedException();

        // return new Order(
        //     OrderId: ,
        //     SourceAppId: ,
        //     OrderDisplayId: ,
        //     Type: ,
        //     DisplayId: ,
        //     CreatedAt: ,
        //     LastEvent: ,
        //     ExternalId: ,
        //     PreparationStartDateTime: ,
        //     SendDelivered: ,
        //     SendTracking: ,
        //     ExtraInfo: ,
        //     CompanyId: ,
        //     Merchant: new OrderMerchant(
        //         Id: ,
        //         Name: ,
        //         Address:
        //     ),
        //     Items: ,
        //     OtherFees: ,
        //     Discounts: ,
        //     Total: new OrderTotal(
        //         Discount: ,
        //         ItemsPrice: ,
        //         OrderAmount: ,
        //         OtherFees:
        //     ),
        //     Payments: new OrderPayment(
        //         Prepaid: ,
        //         Pending: ,
        //         Methods:
        //     ),
        //     Customer: new OrderCustomer(
        //         Id: ,
        //         Name: ,
        //         DocumentNumber: ,
        //         Email: ,
        //         OrdersCountOnMerchant: ,
        //         Phone: new Phone(
        //             Extension: ,
        //             Number:
        //           )
        //     ),
        //     Schedule: ,
        //     Delivery: ,
        //     OrderPriority: ,
        //     Indoor: ,
        //     OrderTiming:
        // );
    }

    public static OrderUpdateStatus FromIfood(this IfoodWebhookRequest request)
    {
        return new OrderUpdateStatus(
            OrderId: request.OrderId,
            SourceAppId: "ifood",
            Type: OrderEventType.CONFIRMED,
            CreateAt: request.CreatedAt,
            FromIntegration: true
        );
    }
}
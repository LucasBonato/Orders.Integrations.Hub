using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity.Merchant;
using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity.Payment;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application;

public class IfoodOrderMapper : IOrderMapper<IfoodOrder>
{
    public Order Mapper(IfoodOrder order)
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
}
using Orders.Integrations.Hub.Modules.Common.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity;
using Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity.Merchant;
using Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity.Payment;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application;

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
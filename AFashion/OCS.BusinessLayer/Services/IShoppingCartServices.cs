using OCS.BusinessLayer.Models;
using System.Collections.Generic;

namespace OCS.BusinessLayer.Services
{
    public interface IShoppingCartServices
    {
        ProductOrderModel AddOrUpdateOrder(ProductOrderModel orderModel, string userName);

        void DeleteOrder(ProductOrderModel order, string userName);

        IEnumerable<ProductOrderModel> GetAllOrders(string userName);
    }
}

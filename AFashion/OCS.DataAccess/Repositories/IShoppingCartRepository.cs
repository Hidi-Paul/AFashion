using OCS.DataAccess.DTO;
using System;

namespace OCS.DataAccess.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart AddOrUpdate(ShoppingCart entity);

        ShoppingCart GetByUserName(string userName);

        void Delete(ProductOrder entity, string userName);
    }
}

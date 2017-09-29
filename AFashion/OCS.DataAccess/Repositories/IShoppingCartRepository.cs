using OCS.DataAccess.DTO;

namespace OCS.DataAccess.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart AddOrUpdateShoppingCart(ShoppingCart entity);
        ProductOrder AddOrUpdateOrder(ProductOrder entity);
        ShoppingCart GetShoppingCartByUserName(string userName);
        void DeleteOrder(ProductOrder entity, string userName);
    }
}

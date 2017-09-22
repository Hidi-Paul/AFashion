using OCS.DataAccess.DTO;

namespace OCS.DataAccess.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart AddOrUpdate(ShoppingCart entity);

        ProductOrder AddOrUpdate(ProductOrder entity);

        ShoppingCart GetByUserName(string userName);

        void Delete(ProductOrder entity, string userName);
    }
}

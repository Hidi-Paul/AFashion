using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System.Data.Entity;
using System.Linq;


namespace OCS.DataAccess.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IFashionContext DbCon;

        public ShoppingCartRepository(IFashionContext dbCon)
        {
            this.DbCon = dbCon;
        }

        public ShoppingCart AddOrUpdate(ShoppingCart entity)
        {
            var set = DbCon.ShoppingCarts;

            ShoppingCart item = set.Find(entity.UserName);
            if (item != null)
            {
                set.Attach(item);
                DbCon.Entry(entity).State = EntityState.Modified;
                DbCon.SaveChanges();
            }
            else
            {
                set.Add(entity);
                DbCon.SaveChanges();
            }
            return entity;
        }

        public ShoppingCart GetByUserName(string userName)
        {
            var set = DbCon.ShoppingCarts;

            ShoppingCart item = set.Where(x => x.UserName.Equals(userName))
                                   .Include("ProductOrders")
                                   .FirstOrDefault();
            return item;
        }

        public void Delete(ProductOrder entity, string userName)
        {
            var set = DbCon.ShoppingCarts;

            ShoppingCart item = set.Find(userName);

            ProductOrder order = item.ProductOrders.Where(x => x.Product.Equals(entity.Product))
                                                   .FirstOrDefault();
            if (order != null)
            {
                DbCon.Entry(order).State = EntityState.Deleted;
                DbCon.SaveChanges();
            }
        }
    }
}

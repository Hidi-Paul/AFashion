using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System.Data.Entity;
using System.Linq;


namespace OCS.DataAccess.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IFashionContext dbContext;
        private readonly DbSet<ShoppingCart> dbShoppingCartSet;
        private readonly DbSet<ProductOrder> dbProductOrderSet;

        public ShoppingCartRepository(IFashionContext dbContext)
        {
            this.dbContext = dbContext;
            dbShoppingCartSet = dbContext.ShoppingCarts;
            dbProductOrderSet = dbContext.ProductOrders;
        }

        public ShoppingCart AddOrUpdateShoppingCart(ShoppingCart entity)
        {
            if (dbShoppingCartSet.Contains(entity))
            {
                UpdateShoppingCart(entity);
            }
            else
            {
                InsertShoppingCart(entity);
            }
            return entity;
        }

        private void UpdateShoppingCart(ShoppingCart entity)
        {
            dbShoppingCartSet.Attach(entity);
            dbContext.SetModified(entity);
            dbContext.SaveChanges();
        }

        private void InsertShoppingCart(ShoppingCart entity)
        {
            dbShoppingCartSet.Add(entity);
            dbContext.SaveChanges();
        }

        public ProductOrder AddOrUpdateOrder(ProductOrder entity)
        {
            if (dbProductOrderSet.Contains(entity))
                UpdateProductOrder(entity);
            else
                InsertProductOrder(entity);
            return entity;
        }

        private void UpdateProductOrder(ProductOrder entity)
        {
            dbProductOrderSet.Attach(entity);
            dbContext.SetModified(entity);
            dbContext.SaveChanges();
        }

        private void InsertProductOrder(ProductOrder entity)
        {
            dbProductOrderSet.Add(entity);
            dbContext.SaveChanges();
        }

        public ShoppingCart GetShoppingCartByUserName(string userName)
        {
            ShoppingCart entity = dbShoppingCartSet.Include("ProductOrders")
                                                   .Include("ProductOrders.Product")
                                                   .Where(x => x.UserName.Equals(userName))
                                                   .FirstOrDefault();
            if (entity == null)
            {
                entity = new ShoppingCartNotFound();
            }
            return entity;
        }

        public void DeleteOrder(ProductOrder entity, string userName)
        {
            ShoppingCart cart = GetShoppingCartByUserName(userName);

            if (cart.ProductOrders.Contains(entity))
            {
                Delete(entity);
            }
        }

        private void Delete(object entity)
        {
            dbContext.SetDeleted(entity);
            dbContext.SaveChanges();
        }
    }
}

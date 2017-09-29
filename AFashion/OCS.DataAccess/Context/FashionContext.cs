using OCS.DataAccess.DTO;
using System.Data.Entity;

namespace OCS.DataAccess.Context
{
    public class FashionContext : DbContext, IFashionContext, IDbContext
    {
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public FashionContext() : base("name=AFashionDbCon")
        {

        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void SetDeleted(object entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }
    }
}
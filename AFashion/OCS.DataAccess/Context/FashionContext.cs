using OCS.DataAccess.DTO;
using System.Data.Entity;

namespace OCS.DataAccess.Context
{
    public class FashionContext : DbContext, IFashionContext, IDbContext
    {
        public FashionContext() : base("name=AFashionDbCon")
        {

        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}
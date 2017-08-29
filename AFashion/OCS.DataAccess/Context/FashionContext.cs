using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCS.DataAccess.Context
{
    public class FashionContext : DbContext
    {
        public FashionContext() : base("name=AFashionDbCon")
        {

        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}

﻿using OCS.DataAccess.DTO;
using System.Data.Entity;

namespace OCS.DataAccess.Context
{
    public interface IFashionContext : IDbContext
    {
        DbSet<Brand> Brands { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductOrder> ProductOrders { get; set; }
        DbSet<ShoppingCart> ShoppingCarts { get; set; }

        void SetModified(object entity);
        void SetDeleted(object entity);
    }
}

using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OCS.DataAccess.Repositories
{
    public class ProductRepository : IEntityRepository<Product>
    {
        private readonly IFashionContext DbCon;

        public ProductRepository(IFashionContext dbCon)
        {
            this.DbCon = dbCon;
        }

        public Product AddOrUpdate(Product entity)
        {
            var set = DbCon.Products;

            Product item = set.Find(entity.ID);
            if (item != null)
            {
                set.Attach(entity);
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

        public ICollection<Product> GetAll()
        {
            var items = DbCon.Products.Include("Category")
                                    .Include("Brand");
            return items.ToList();
        }

        public Product GetByID(Guid id)
        {
            var set = DbCon.Products;

            var item = set.Include("Category")
                         .Include("Brand")
                         .Where(x => x.ID.Equals(id))
                         .FirstOrDefault();
            return item;
        }

        public Product GetByName(string name)
        {
            var set = DbCon.Products;

            var item = set.Include("Category")
                          .Include("Brand")
                          .Where(x => x.Name.Equals(name))
                          .FirstOrDefault();
            return item;
        }
    }
}

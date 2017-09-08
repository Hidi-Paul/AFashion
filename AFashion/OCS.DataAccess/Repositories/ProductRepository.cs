using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OCS.DataAccess.Repositories
{
    public class ProductRepository : IEntityRepository<Product>
    {
        private readonly FashionContext DbCon = new FashionContext();

        public ProductRepository(FashionContext dbCon)
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
            }
            else
            {
                set.Add(entity);
            }
            return entity;
        }

        public IEnumerable<Product> GetAll()
        {
            var items = DbCon.Products.Include("Category")
                                    .Include("Brand");
            return items;
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

        public int SaveChanges()
        {
            var changeCount = DbCon.SaveChanges();
            return changeCount;
        }
    }
}

using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OCS.DataAccess.Repositories
{
    public class ProductRepo : IEntityRepository<Product>
    {
        private readonly IFashionContext dbContext;
        private readonly DbSet<Product> dbSet;

        public ProductRepo(IFashionContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Products;
        }

        public Product GetByID(Guid id)
        {
            Product entity = dbSet.Include("Brand")
                                  .Include("Category")
                                  .Where(x => x.ID.Equals(id))
                                  .FirstOrDefault();
            if (entity == null)
            {
                entity = new ProductNotFound();
            }
            return entity;
        }

        public Product GetByName(string name)
        {
            Product entity = dbSet.Include("Brand")
                                  .Include("Category")
                                  .Where(x => x.Name.Equals(name))
                                  .FirstOrDefault();
            if (entity == null)
            {
                return new ProductNotFound();
            }
            return entity;
        }

        public ICollection<Product> GetAll()
        {
            ICollection<Product> entities = dbSet.Include("Brand")
                                                 .Include("Category")
                                                 .ToList();
            return entities;
        }

        public Product AddOrUpdate(Product entity)
        {
            if (dbSet.Contains(entity))
            {
                Update(entity);
            }
            else
            {
                Insert(entity);
            }
            return entity;
        }

        private void Update(Product entity)
        {
            dbSet.Attach(entity);
            dbContext.SetModified(entity);
            dbContext.SaveChanges();
        }

        private void Insert(Product entity)
        {
            dbSet.Add(entity);
            dbContext.SaveChanges();
        }
    }
}

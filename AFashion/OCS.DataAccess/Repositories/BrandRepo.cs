using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OCS.DataAccess.Repositories
{
    public class BrandRepo : IEntityRepository<Brand>
    {
        private readonly IFashionContext dbContext;
        private readonly DbSet<Brand> dbSet;

        public BrandRepo(IFashionContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Brands;
        }

        public Brand GetByID(Guid id)
        {
            Brand entity = dbSet.Where(item => item.ID == id).FirstOrDefault();
            if (entity == null)
                entity = new BrandNotFound();
            return entity;
        }

        public Brand GetByName(string name)
        {
            Brand entity = dbSet.Where(item => item.Name.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
            if (entity == null)
                entity = new BrandNotFound();
            return entity;
        }

        public ICollection<Brand> GetAll()
        {
            ICollection<Brand> entities = dbSet.ToList();
            
            return entities;
        }

        public Brand AddOrUpdate(Brand entity)
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

        private void Update(Brand entity)
        {
            dbSet.Attach(entity);
            dbContext.SetModified(entity);
            dbContext.SaveChanges();
        }

        private void Insert(Brand entity)
        {
            dbSet.Add(entity);
            dbContext.SaveChanges();
        }
    }
}

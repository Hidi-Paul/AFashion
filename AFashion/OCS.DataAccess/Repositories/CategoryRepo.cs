using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OCS.DataAccess.Repositories
{
    public class CategoryRepo : IEntityRepository<Category>
    {
        private readonly IFashionContext dbContext;
        private readonly DbSet<Category> dbSet;

        public CategoryRepo(IFashionContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Categories;
        }

        public Category GetByID(Guid id)
        {
            Category entity = dbSet.Where(item => item.ID == id).FirstOrDefault();
            if (entity == null)
                entity = new CategoryNotFound();
            return entity;
        }

        public Category GetByName(string name)
        {
            Category entity = dbSet.Where(item => item.Name.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
            if (entity == null)
                entity = new CategoryNotFound();
            return entity;
        }

        public ICollection<Category> GetAll()
        {
            ICollection<Category> entities = dbSet.ToList();

            return entities;
        }

        public Category AddOrUpdate(Category entity)
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

        private void Update(Category entity)
        {
            dbSet.Attach(entity);
            dbContext.SetModified(entity);
            dbContext.SaveChanges();
        }

        private void Insert(Category entity)
        {
            dbSet.Add(entity);
            dbContext.SaveChanges();
        }
    }
}

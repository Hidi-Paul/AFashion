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
        private readonly IFashionContext DbCon;

        public CategoryRepo(IFashionContext DbCon)
        {
            this.DbCon = DbCon;
        }

        public Category AddOrUpdate(Category entity)
        {
            var set = GetDbSet();

            Category result;
            if (set.Contains(entity))
            {
                result = set.Attach(entity);
                DbCon.Entry(entity).State = EntityState.Modified;
                DbCon.SaveChanges();
            }
            else
            {
                result = set.Add(entity);
                DbCon.SaveChanges();
            }
            return result;
        }

        public ICollection<Category> GetAll()
        {
            var set = GetDbSet();
            return set.ToList();
        }

        public Category GetByID(Guid id)
        {
            var set = GetDbSet();
            return set.Where(item => item.ID == id).FirstOrDefault();
        }

        public Category GetByName(string name)
        {
            var set = GetDbSet();
            return set.Where(item => item.Name.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
        }

        private DbSet<Category> GetDbSet()
        {
            var set = DbCon.Categories;
            return set;
        }
    }
}

using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCS.DataAccess.Repositories
{
    public class BrandRepo : IEntityRepository<Brand>
    {
        private readonly IFashionContext DbContext;
        private readonly DbSet<Brand> DbSet;
        
        public BrandRepo(IFashionContext DbCon)
        {
            this.DbContext = DbCon;
            this.DbSet = DbCon.Brands;
        }

        public Brand AddOrUpdate(Brand entity)
        {
            if (DbSet.Contains(entity))
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
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            DbContext.SaveChanges();
        }

        private void Insert(Brand entity)
        {
            DbSet.Add(entity);
            DbContext.SaveChanges();
        }

        public ICollection<Brand> GetAll()
        {
            var set = DbSet;
            return set.ToList();
        }

        public Brand GetByID(Guid id)
        {
            var set = DbSet;
            return set.Where(item => item.ID == id).FirstOrDefault();
        }

        public Brand GetByName(string name)
        {
            var set = DbSet;
            return set.Where(item => item.Name.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
        }
    }
}

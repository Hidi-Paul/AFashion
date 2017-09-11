using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OCS.DataAccess.Repositories
{

    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IFashionContext DbCon;

        public EntityRepository(IFashionContext dbCon)
        {
            this.DbCon = dbCon;
        }

        public TEntity AddOrUpdate(TEntity entity)
        {
            var set = DbCon.Set<TEntity>();
            if (set != null)
            {
                TEntity result;
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
            return null;
        }

        public IEnumerable<TEntity> GetAll()
        {
            var set = DbCon.Set<TEntity>();
            if (set != null)
            {
                return set.ToList();
            }
            return new List<TEntity>();
        }

        public TEntity GetByID(Guid id)
        {
            var set = DbCon.Set<TEntity>();
            if (set != null)
            {
                return set.Where(item => item.ID == id).FirstOrDefault();
            }
            return null;
        }

        public TEntity GetByName(string name)
        {
            var set = DbCon.Set<TEntity>();
            if (set != null)
            {
                return set.Where(item => item.Name.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
            }
            return null;
        }
    }
}

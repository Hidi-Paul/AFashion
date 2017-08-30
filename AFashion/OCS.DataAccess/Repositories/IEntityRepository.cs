using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace OCS.DataAccess.Repositories
{
    public interface IEntityRepository<TEntity> where TEntity: class, IEntity
    {
        TEntity GetByID(Guid id);
        TEntity GetByName(string name);
        IEnumerable<TEntity> GetAll();
        TEntity AddOrUpdate(TEntity entity);
        int SaveChanges();
    }
}

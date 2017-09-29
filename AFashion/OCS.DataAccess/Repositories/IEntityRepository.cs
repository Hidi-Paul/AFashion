using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;

namespace OCS.DataAccess.Repositories
{
    public interface IEntityRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetByID(Guid id);
        TEntity GetByName(string name);
        ICollection<TEntity> GetAll();
        TEntity AddOrUpdate(TEntity entity);
    }
}

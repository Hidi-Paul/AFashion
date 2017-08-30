using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;

namespace OCS.BusinessLayer.Services
{
    public interface IEntityServices<TEntity, TModel> where TEntity : IEntity
    {
        IEnumerable<TModel> GetAll();
    }
}

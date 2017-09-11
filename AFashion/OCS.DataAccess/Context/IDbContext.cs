using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace OCS.DataAccess.Context
{
    public interface IDbContext : IDisposable
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}

using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using P21_latest_template.Services;
using System;

namespace P21_latest_template.Repositories
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        bool Create(TEntity entity);
        bool Delete(TEntity entity);
        TEntity Get(TKey id);
        bool Update(TEntity entity);
    }

    public class GenericRepository<TEntity> where TEntity : class
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<GenericRepository<TEntity>> Logger;

        public GenericRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<GenericRepository<TEntity>>();
        }

        public virtual TEntity Get(int id)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Get<TEntity>(id);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, id);
            }
            return null;
        }

        public virtual bool Create(TEntity entity)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Insert(entity);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, entity);
            }
            return false;
        }

        public virtual bool Update(TEntity entity)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Update(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, entity);
            }
            return false;
        }

        public virtual bool Delete(TEntity entity)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Delete(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, entity);
            }
            return false;
        }
    }
}

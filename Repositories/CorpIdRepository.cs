using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Repositories
{
    public interface ICorpIdRepository
    {
        bool Create(CorpId entity);
        bool Delete(CorpId entity);
        CorpId Get(string company_id,
            decimal address_id);
        bool Update(CorpId entity);
    }

    public class CorpIdRepository : ICorpIdRepository
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<CorpIdRepository> Logger;

        public CorpIdRepository(IDbConnectionService conn,
            ILoggerFactory loggerFactory)
        {
            DbConn = conn;
            Logger = loggerFactory.CreateLogger<CorpIdRepository>();
        }

        public virtual bool Create(CorpId entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

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

        public virtual bool Delete(CorpId entity)
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

        public virtual CorpId Get(string company_id,
            decimal address_id)
        {
            string sql = @"
select	*
from	corp_id
where	company_id = @company_id
		and address_id = @address_id
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    var data = conn.Query<CorpId>(sql, new
                    {
                        company_id,
                        address_id
                    });

                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, company_id, address_id);
            }
            return null;
        }

        public virtual bool Update(CorpId entity)
        {
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

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
    }
}

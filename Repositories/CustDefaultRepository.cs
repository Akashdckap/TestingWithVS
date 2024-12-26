using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using Serilog.Core;
using System;

namespace P21_latest_template.Repositories
{
    public interface ICustDefaultRepository : IRepository<CustDefault, string>
    {

    }

    public class CustDefaultRepository : GenericRepository<CustDefault>, ICustDefaultRepository
    {
        public CustDefaultRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public CustDefault Get(string id)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Get<CustDefault>(id);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, id);
            }
            return null;
        }
    }
}

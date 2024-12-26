using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Services
{
    public interface IDbConnectionService
    {
        IDbConnection GetP21DbConnection();
    }

    public class DbConnectionService : IDbConnectionService
    {
        private readonly string _connStr;

        public DbConnectionService(string connStr)
        {
            _connStr = connStr;
        }

        public virtual IDbConnection GetP21DbConnection()
        {
            return new SqlConnection(_connStr);
        }
    }
}

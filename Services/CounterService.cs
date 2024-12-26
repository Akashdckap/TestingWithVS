using Dapper;
using Microsoft.Extensions.Logging;
using System;

namespace P21_latest_template.Services
{
    public interface ICounterService
    {
        int GetNextCounter(string counterId);
    }

    public class CounterService : ICounterService
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<CounterService> Logger;

        public CounterService(
            IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<CounterService>();
        }

        public int GetNextCounter(string counterId)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    string sql = "p21_ads_get_counter";
                    return conn.ExecuteScalar<int>(sql, new { CounterId = counterId },
                        commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, counterId);
            }
            return -1;
        }
    }
}

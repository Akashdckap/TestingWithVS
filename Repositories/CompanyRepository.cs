using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using System;


namespace P21_latest_template.Repositories
{
    public interface ICompanyRepository : IRepository<Company, string>
    {

    }

    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public Company Get(string id)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Get<Company>(id);
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

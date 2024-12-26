using Dapper;
using Microsoft.Extensions.Logging;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using Serilog.Core;
using System;
using System.Linq;

namespace P21_latest_template.Repositories
{
    public interface IPriceLibraryRepository : IRepository<PriceLibrary, int>
    {
        PriceLibrary GetByPriceLibraryId(string price_library_id);
    }

    public class PriceLibraryRepository : GenericRepository<PriceLibrary>, IPriceLibraryRepository
    {
        public PriceLibraryRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public PriceLibrary GetByPriceLibraryId(string price_library_id)
        {
            string sql = @"
select	top 1 *
from	price_library
where   price_library_id = @price_library_id
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    var data = conn.Query<PriceLibrary>(sql, new
                    {
                        price_library_id
                    });

                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, price_library_id);
            }
            return null;
        }
    }
}

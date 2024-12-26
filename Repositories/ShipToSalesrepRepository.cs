using Dapper;
using Microsoft.Extensions.Logging;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using Serilog.Core;
using System;
using System.Linq;

namespace P21_latest_template.Repositories
{
    public interface IShipToSalesrepRepository
    {
        ShipToSalesrep Get(string company_id, decimal ship_to_id, string salesrep_id);
        bool Create(ShipToSalesrep entity);
        bool Delete(ShipToSalesrep entity);
        bool Update(ShipToSalesrep entity);
    }

    public class ShipToSalesrepRepository : GenericRepository<ShipToSalesrep>, IShipToSalesrepRepository
    {
        public ShipToSalesrepRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public ShipToSalesrep Get(string company_id,
            decimal ship_to_id, string salesrep_id)
        {
            string sql = @"
select  *
from    ship_to_salesrep
where   company_id = @company_id
        and ship_to_id = @ship_to_id
        and salesrep_id = @salesrep_id
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    var data = conn.Query<ShipToSalesrep>(sql, new
                    {
                        company_id,
                        ship_to_id,
                        salesrep_id
                    });
                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, company_id, ship_to_id, salesrep_id);
            }
            return null;
        }

        public override bool Create(ShipToSalesrep entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Create(entity);
        }

        public override bool Update(ShipToSalesrep entity)
        {
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Update(entity);
        }
    }
}

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
    public interface IShipToRepository
    {
        bool Create(ShipTo entity);
        bool Delete(ShipTo entity);
        ShipTo Get(string company_id, decimal ship_to_id);
        bool Update(ShipTo entity);
    }

    public class ShipToRepository : GenericRepository<ShipTo>, IShipToRepository
    {
        public ShipToRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public ShipTo Get(string company_id, decimal ship_to_id)
        {
            string sql = @"
select  *
from    ship_to 
where   company_id = @company_id 
        and ship_to_id = @ship_to_id
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    var data = conn.Query<ShipTo>(sql, new
                    {
                        company_id,
                        ship_to_id
                    });
                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, company_id, ship_to_id);
            }
            return null;
        }

        public override bool Create(ShipTo entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;
            if (string.IsNullOrEmpty(entity.created_by))
                entity.created_by = "admin";
            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";
            return base.Create(entity);
        }

        public override bool Update(ShipTo entity)
        {
            entity.date_last_modified = DateTime.Now;
            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";
            return base.Update(entity);
        }
    }
}

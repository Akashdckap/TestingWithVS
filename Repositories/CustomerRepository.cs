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
    public interface ICustomerRepository
    {
        Customer Get(string company_id, decimal customer_id);
        bool Create(Customer entity);
        bool Update(Customer entity);
    }

    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public virtual Customer Get(string company_id, decimal customer_id)
        {
            string sql = @"
select  *
from    customer
where   company_id = @company_id
		and customer_id = @customer_id
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    var data = conn.Query<Customer>(sql, new { company_id, customer_id });
                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, company_id, customer_id);
            }
            return null;
        }

        public override bool Create(Customer entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.created_by))
                entity.created_by = "admin";

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Create(entity);
        }

        public override bool Update(Customer entity)
        {
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Update(entity);
        }
    }
}

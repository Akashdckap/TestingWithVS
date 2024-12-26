using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using System;
using System.Linq;

namespace P21_latest_template.Repositories
{
    public interface IOeContactCustomerRepository
    {
        bool Create(OeContactCustomer entity);
        bool Delete(OeContactCustomer entity);
        OeContactCustomer Get(string company_id,
            decimal customer_id, string contact_id);
        bool Update(OeContactCustomer entity);
    }

    public class OeContactCustomerRepository : IOeContactCustomerRepository
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<OeContactCustomerRepository> Logger;

        public OeContactCustomerRepository(IDbConnectionService conn,
            ILoggerFactory loggerFactory)
        {
            DbConn = conn;
            Logger = loggerFactory.CreateLogger<OeContactCustomerRepository>();
        }

        public virtual bool Create(OeContactCustomer entity)
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

        public virtual bool Delete(OeContactCustomer entity)
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

        public virtual OeContactCustomer Get(string company_id,
            decimal customer_id, string contact_id)
        {
            string sql = @"
select	*
from	oe_contacts_customer
where	company_id = @company_id
		and customer_id = @customer_id
		and contact_id = @contact_id
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    var data = conn.Query<OeContactCustomer>(sql, new
                    {
                        company_id,
                        customer_id,
                        contact_id
                    });

                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, company_id, customer_id, contact_id);
            }
            return null;
        }

        public virtual bool Update(OeContactCustomer entity)
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

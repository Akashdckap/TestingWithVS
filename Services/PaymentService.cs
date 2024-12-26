using Dapper;
using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using P21_latest_template.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Services
{
    public class PaymentService : IPaymentService
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<PaymentService> Logger;

        public PaymentService(
            IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<PaymentService>();
        }

        public virtual IEnumerable<PaymentType> GetPaymentTypes()
        {
            string sql = @"
select	company_id
		,payment_type_id
		,payment_type_desc
from	payment_types
where	delete_flag <> 'Y'
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    return conn.Query<PaymentType>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
            return null;
        }
    }
}

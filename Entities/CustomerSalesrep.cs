using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("customer_salesrep")]
    public class CustomerSalesrep
    {
        [ExplicitKey]
        public int customer_salesrep_uid { get; set; }
        public string company_id { get; set; }
        public decimal customer_id { get; set; }
        public string salesrep_id { get; set; }
        public decimal commission_percentage { get; set; }
        public string primary_salesrep_flag { get; set; }
        public int row_status_flag { get; set; }
        public DateTime date_created { get; set; }
        public string created_by { get; set; }
        public DateTime date_last_modified { get; set; }
        public string last_maintained_by { get; set; }

        public CustomerSalesrep()
        {
            commission_percentage = 100M;
            primary_salesrep_flag = "Y";
            row_status_flag = 704;
            date_created = DateTime.Now;
            created_by = "admin";
            date_last_modified = DateTime.Now;
            last_maintained_by = "admin";
        }
    }
}

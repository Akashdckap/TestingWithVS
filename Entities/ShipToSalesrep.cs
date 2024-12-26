using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("ship_to_salesrep")]
    public class ShipToSalesrep
    {
        [ExplicitKey]
        public string company_id { get; set; }
        [ExplicitKey]
        public decimal ship_to_id { get; set; }
        [ExplicitKey]
        public string salesrep_id { get; set; }
        public string delete_flag { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
        public string last_maintained_by { get; set; }
        public string primary_salesrep { get; set; }
        public decimal commission_percentage { get; set; }
        public string primary_service_rep { get; set; }

        public ShipToSalesrep()
        {
            delete_flag = "N";
            date_created = DateTime.Now;
            date_last_modified = DateTime.Now;
            last_maintained_by = "?";
            commission_percentage = 100;
            primary_salesrep = "Y";
            primary_service_rep = "N";
        }
    }
}

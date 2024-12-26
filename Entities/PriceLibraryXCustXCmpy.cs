using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("price_library_x_cust_x_cmpy")]
    public class PriceLibraryXCustXCmpy
    {
        [ExplicitKey]
        public int price_lib_x_cust_x_cmpy_uid { get; set; }
        public string company_id { get; set; }
        public decimal customer_id { get; set; }
        public int price_library_uid { get; set; }
        public int sequence_number { get; set; }
        public int row_status_flag { get; set; }
        public DateTime date_last_modified { get; set; }
        public DateTime date_created { get; set; }
        public string last_maintained_by { get; set; }
        public string web_based_pricing { get; set; }
        public string distributor_net_flag { get; set; }

        public PriceLibraryXCustXCmpy()
        {
            sequence_number = 1;
            row_status_flag = 704;
            date_last_modified = DateTime.Now;
            date_created = DateTime.Now;
            last_maintained_by = "admin";
            web_based_pricing = "N";
            distributor_net_flag = "N";
        }
    }
}

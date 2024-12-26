using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class GetAgingReport
    {
        public string company_id { get; set; }
        public decimal customer_id { get; set; }
        public decimal total_amount { get; set; }
        public DateTime invoice_date { get; set; }
        public DateTime net_due_date { get; set; }
        public decimal invoice_no { get; set; }
        public DateTime order_date { get; set; }
        public decimal order_no { get; set; }
        public string company_name { get; set; }
        public string name { get; set; }
        public string currency_desc { get; set; }
        public decimal memo_amount { get; set; }
        public decimal bucket0 { get; set; }
        public decimal bucket1 { get; set; }
        public decimal bucket2 { get; set; }
        public decimal bucket3 { get; set; }
        public decimal bucket4 { get; set; }
        public decimal original_amount { get; set; }
        public decimal total_due { get; set; }
        public string branch_description { get; set; }

        public decimal amount_paid { get; set; }
    }
}

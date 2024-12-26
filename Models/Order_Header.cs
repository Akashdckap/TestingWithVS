using P21_latest_template.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class Order_Header
    {
        public string order_no { get; set; }
        public DateTime order_date { get; set; }
        public string status { get; set; }
        public decimal sub_total { get; set; }
        public decimal tax { get; set; }
        public decimal total { get; set; }

        public decimal lines_count { get; set; }
        public decimal ship2_id { get; set; }
        public string ship2_name { get; set; }
        public string po_no { get; set; }
        public string name { get; set; }

        public string web_reference_no { get; set; }
        public decimal shipping_cost { get; set; }
        public DateTime? expiration_date { get; set; }
        public DateTime date_created { get; set; }
        public string delivery_instructions { get; set; }
        public int total_records { get; set; }
    }

    public class Get_Order_Header
    {
        [Required]
        public decimal customer_id { get; set; }
        [Required]
        public DateTime from_date { get; set; }
        [Required]
        public DateTime to_date { get; set; }
        //[ValidOfflineOrderTypes(ErrorMessage = "Not a valid Order Type")]
        public string order_type { get; set; }
    }
}

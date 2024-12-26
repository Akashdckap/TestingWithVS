using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class OrderDetails
    {
        public decimal order_no { get; set; }
        public decimal shipping_cost { get; set; }
        public string status { get; set; }
        public decimal tax { get; set; }
        public string payment_desc { get; set; }
        public string shipping_method { get; set; }
        public DateTime order_date { get; set; }
        public DateTime? expiration_date { get; set; }
        public string ship2_name { get; set; }
        public string ship2_city { get; set; }
        public string ship_to_phone { get; set; }
        public string ship2_state { get; set; }
        public string ship2_add1 { get; set; }
        public string ship2_add2 { get; set; }
        public string ship2_add3 { get; set; }
        public string ship2_country { get; set; }
        public string ship2_zip { get; set; }
        public string name { get; set; }
        public string ship2_email_address { get; set; }
        public string mail_address1 { get; set; }
        public string mail_address2 { get; set; }
        public string mail_address3 { get; set; }
        public string mail_city { get; set; }
        public string mail_country { get; set; }
        public string mail_postal_code { get; set; }
        public string mail_state { get; set; }
        public string central_phone_number { get; set; }
        public string email_address { get; set; }
        public char cancel_flag { get; set; }
        public IEnumerable<OrderLines> orderLines { get; set; }

    }
    public class OrderLines
    {
        public decimal inv_mast_uid { get; set; }
        public string item_desc { get; set; }
        public string item_id { get; set; }
        public decimal unit_price { get; set; }
        public decimal extended_price { get; set; }
        public string unit_of_measure { get; set; }
        public string disposition { get; set; }
        public int qty_ordered { get; set; }
        public int qty_allocated { get; set; }
        public DateTime? promise_date { get; set; }
        public DateTime? expedite_date { get; set; }
        public DateTime? required_date { get; set; }
        public string pricing_unit { get; set; }
        public int pricing_unit_size { get; set; }
        public int unit_size { get; set; }
        public int qty_invoiced { get; set; }
        public int qty_canceled { get; set; }
        public int qty_open { get; set; }
    }
}

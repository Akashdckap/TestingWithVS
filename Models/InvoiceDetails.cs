using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class InvoiceDetails
    {
        public InvoiceInfo invoiceInfo { get; set; }
        public IEnumerable<InvoiceLines> invoiceLines { get; set; }

    }

    public class InvoiceInfo
    {
        public string invoice_no { get; set; }
        public DateTime invoice_date { get; set; }
        public DateTime net_due_date { get; set; }
        public string terms_desc { get; set; }
        public DateTime terms_due_date { get; set; }
        public decimal terms_amount { get; set; }
        public DateTime disc_due_date { get; set; }
        public decimal disc_amount { get; set; }
        public string branch_desc { get; set; }
        public int branch_id { get; set; }
        public string pick_ticket_no { get; set; }
        public string carrier_name { get; set; }
        public string customer_company_name { get; set; }

        public int carrier_id { get; set; }
        public string tracking_no { get; set; }
        public char direct_shipment { get; set; }
        public string company_phone { get; set; }
        public string company_state { get; set; }
        public string company_country { get; set; }
        public string company_postal_code { get; set; }
        public string company_address1 { get; set; }
        public string company_city { get; set; }
        public string bill2_address1 { get; set; }
        public string bill2_address2 { get; set; }
        public string bill2_address3 { get; set; }
        public string bill2_phone { get; set; }
        public string bill2_state { get; set; }
        public string bill2_country { get; set; }
        public string bill2_postal_code { get; set; }
        public string bill2_city { get; set; }
        public string ship2_address1 { get; set; }
        public string ship2_address2 { get; set; }
        public string ship2_address3 { get; set; }
        public string ship2_phone { get; set; }
        public string ship2_state { get; set; }
        public string ship2_country { get; set; }
        public string ship2_postal_code { get; set; }
        public string ship2_city { get; set; }
        public string order_no { get; set; }
        public string po_no { get; set; }
        public int salesrep_id { get; set; }
        public string salesrep_name { get; set; }
        public string taker { get; set; }
        public string ordered_by { get; set; }
        public string order_date { get; set; }
        public string amount_paid { get; set; }


        //public CompanyAddress comapnyAddress { get; set; }
        //public BillingAddress billingAddress { get; set; }
        //public ShippingAddress shippingAddress { get; set; }
        //public OrderInfo orderInfo { get; set; }
        //public ShipmentInfo shipmentInfo { get; set; }
        //public BranchInfo branchInfo { get; set; }
    }

    public class BranchInfo
    {
        public string branch_desc { get; set; }
        public int branch_id { get; set; }
    }

    public class ShipmentInfo
    {
        public string pick_ticket_no { get; set; }
        public string carrier_name { get; set; }
        public int carrier_id { get; set; }
        public string tracking_no { get; set; }
        public char direct_shipment { get; set; }
    }

    public class CompanyAddress
    {
        public string company_phone { get; set; }
        public string company_state { get; set; }
        public string company_country { get; set; }
        public string company_postal_code { get; set; }
        public string company_address1 { get; set; }
        public string company_city { get; set; }
    }

    public class BillingAddress
    {
        public string bill2_address1 { get; set; }
        public string bill2_address2 { get; set; }
        public string bill2_address3 { get; set; }
        public string bill2_phone { get; set; }
        public string bill2_state { get; set; }
        public string bill2_country { get; set; }
        public string bill2_postal_code { get; set; }
        public string bill2_city { get; set; }
    }

    public class ShippingAddress
    {
        public string ship2_address1 { get; set; }
        public string ship2_address2 { get; set; }
        public string ship2_address3 { get; set; }
        public string ship2_phone { get; set; }
        public string ship2_state { get; set; }
        public string ship2_country { get; set; }
        public string ship2_postal_code { get; set; }
        public string ship2_city { get; set; }
    }

    public class InvoiceLines
    {
        public decimal unit_price { get; set; }
        public decimal extended_price { get; set; }
        public string item_id { get; set; }
        public decimal inv_mast_uid { get; set; }
        public string item_desc { get; set; }
        public string unit_of_measure { get; set; }
        public decimal ordered { get; set; }
        public decimal remaining { get; set; }
        public decimal shipped { get; set; }
        public int invoice_line_type { get; set; }
    }

    public class OrderInfo
    {
        public string order_no { get; set; }
        public string po_no { get; set; }
        public int salesrep_id { get; set; }
        public string salesrep_name { get; set; }
        public string taker { get; set; }
        public string ordered_by { get; set; }
    }

    //Added By Vanditha 
    public class Order_Header_info
    {
        public string order_no { get; set; }
        public DateTime order_date { get; set; }
        public string ship2_name { get; set; }
        public IEnumerable<ShippingAddress> ShippingDetails { get; set; } = new List<ShippingAddress>();



    }


}

using System.Collections.Generic;

namespace P21_latest_template.Models
{
    //public class OrderLite
    //{
    //    public string order_no { get; set; }
    //    public string date_created { get; set; }
    //    public string delete_flag { get; set; }
    //    public string approved { get; set; }
    //    public string taker { get; set; }
    //    public string web_reference_no { get; set; }
    //}

    public class Order //: OrderLite
    {
        public string company_id { get; set; }
        public string p21_order_id { get; set; }
        public string web_reference_id { get; set; }
        public string delete_flag { get; set; }
        public string cancel_flag { get; set; }
        public string approved { get; set; }
        public string completed { get; set; }
        public string order_type { get; set; }

        public IEnumerable<OrderLine> lines { get; set; } = new List<OrderLine>();
        public IEnumerable<OrderInvoice> invoices { get; set; } = new List<OrderInvoice>();
        public IEnumerable<OrderShipment> shipments { get; set; } = new List<OrderShipment>();
    }

    public class OrderLine
    {
        public decimal qty { get; set; }
        public string item_id { get; set; }
        public decimal unit_price { get; set; }
        public decimal unit_quantity { get; set; }
    }

    public class OrderInvoice
    {
        public string p21_invoice_no { get; set; }
        public IEnumerable<OrderInvoiceLine> items { get; set; } = new List<OrderInvoiceLine>();
    }

    public class OrderInvoiceLine
    {
        public string p21_invoice_no { get; set; }
        public decimal qty { get; set; }
        public string item_id { get; set; }
        public decimal unit_price { get; set; }
    }

    public class OrderShipment
    {
        public decimal? pick_ticket_no { get; set; }
        public decimal? carrier_code { get; set; }
        public string title { get; set; }
        public string tracking_number { get; set; }
        public IEnumerable<OrderShipmentLine> items { get; set; } = new List<OrderShipmentLine>();
    }

    public class OrderShipmentLine
    {
        public decimal? pick_ticket_no { get; set; }
        public decimal qty { get; set; }
        public string item_id { get; set; }
    }

    public class OrderPickTicket
    {
        public decimal pick_ticket_no { get; set; }
        public string company_id { get; set; }
        public decimal? carrier_id { get; set; }
        public string carrier_name { get; set; }
        public string tracking_no { get; set; }
    }
}

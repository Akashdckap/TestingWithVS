using System.Collections.Generic;

namespace P21_latest_template.Models
{
    public class Invoice
    {
        public string p21_invoice_no { get; set; }
        public IEnumerable<InvoiceLine> items { get; set; }
    }

    public class InvoiceLine
    {
        public decimal qty { get; set; }
        public string item_id { get; set; }
        public decimal unit_price { get; set; }
    }

    public class ShipmentLine
    {
        public decimal qty { get; set; }
        public string item_id { get; set; }
    }

    public class InvoiceFlat
    {
        public string company_id { get; set; }
        public string p21_order_id { get; set; }
        public string web_reference_id { get; set; }
        public string p21_invoice_no { get; set; }
        public decimal? qty { get; set; }
        public string item_id { get; set; }
        public decimal? unit_price { get; set; }
        public string delete_flag { get; set; }
        public string cancel_flag { get; set; }
        public string approved { get; set; }
        public string completed { get; set; }
        public string order_type { get; set; }
    }

    public class Shipment
    {
        public decimal? pick_ticket_no { get; set; }
        public decimal? carrier_code { get; set; }
        public string title { get; set; }
        public string tracking_number { get; set; }
        public IEnumerable<ShipmentLine> items { get; set; }
    }

    public class ShipmentFlat
    {
        public string company_id { get; set; }
        public string p21_order_id { get; set; }
        public string web_reference_id { get; set; }
        public decimal? pick_ticket_no { get; set; }
        public decimal? carrier_code { get; set; }
        public string title { get; set; }
        public string tracking_number { get; set; }
        public decimal? qty { get; set; }
        public string item_id { get; set; }
        public string delete_flag { get; set; }
        public string cancel_flag { get; set; }
        public string approved { get; set; }
        public string completed { get; set; }
        public string order_type { get; set; }
    }

    public class InvoiceShipment
    {
        public string company_id { get; set; }
        public string p21_order_id { get; set; }
        public string web_reference_id { get; set; }
        public string delete_flag { get; set; }
        public string cancel_flag { get; set; }
        public string approved { get; set; }
        public string completed { get; set; }
        public string order_type { get; set; }

        public IEnumerable<Invoice> invoices { get; set; }
        public IEnumerable<Shipment> shipments { get; set; }
    }
}

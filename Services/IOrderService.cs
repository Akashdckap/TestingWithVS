using System;
using System.Collections.Generic;
using P21_latest_template.Models;

namespace P21_latest_template.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderPickTicket> GetPickTicketByOrderNo(string order_no);
        IEnumerable<InvoiceShipment> GetInvoiceByDateLastModified(string company_id,
            DateTime? from_date, DateTime? to_date, IEnumerable<string> order_ids);
        IEnumerable<Order> Find(string company_id,
            DateTime? from_date, DateTime? to_date, IEnumerable<string> web_ref_numbers);
        IEnumerable<SalesForceOrder> SalesForceFind(string company_id,
            DateTime? from_date, DateTime? to_date, IEnumerable<string> web_ref_numbers, int page_number, int page_size, int line_count);
        IEnumerable<GetAgingReport> GetAgingReports(decimal customer_id, string v);
        InvoiceDetails GetInvoiceInfo(string invoice_no);

        OrderDetails GetOrderInfo(string order_no);
        IEnumerable<Order_Header> GetCustomerOrderDetails(OfflineQuoteFilterParam order_header);


    }
}
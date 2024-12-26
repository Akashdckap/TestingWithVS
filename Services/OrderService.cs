using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using P21_latest_template.Models;
using P21_latest_template.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Services
{
    public class OrderService : IOrderService
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<OrderService> Logger;

        public OrderService(
            IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<OrderService>();
        }

        public virtual IEnumerable<GetAgingReport> GetAgingReports(decimal customer_id, string year_period)
        {
            #region sql
            string sql = @"SELECT artb_temp.company_id, artb_temp.customer_id,
             artb_temp.invoice_no, 
             artb_temp.total_amount as original_amount, 
             artb_temp.memo_amount, 
             artb_temp.invoice_date, 
             artb_temp.salesrep_name, 
             artb_temp.total_receipts, 
             artb_temp.allowed_amount, 
             artb_temp.salesrep_id, 
             artb_temp.order_date, 
             artb_temp.order_no, 
             artb_temp.date_created, 
             artb_temp.date_last_modified, 
             artb_temp.last_maintained_by, 
             artb_temp.period, 
             artb_temp.year_for_period, 
             company.company_name, 
             artb_temp.terms_desc, 
             artb_temp.bad_debt_amount, 
             artb_temp.terms_amount, 
             artb_temp.amount_paid,
             artb_temp.net_due_date, 
             address_a.mail_address1, 
             NULLIF(address_a.mail_address2, ''), 
             ISNULL(address_a.mail_city, ''), 
             ISNULL(address_a.mail_state, ''), 
             ISNULL(address_a.mail_postal_code, ''), 
             address_a.central_phone_number, 
             address_b.name, 
             address_b.mail_address1, 
             NULLIF(address_b.mail_address2, ''), 
             ISNULL(address_b.mail_city, ''), 
             ISNULL(address_b.mail_state, ''), 
             ISNULL(address_b.mail_postal_code, ''), 
             address_b.central_phone_number, 
             CASE 'I'
                WHEN 'I'
                    THEN ''
                ELSE
                    artb_temp.branch_id
             END AS branch_id,

              bucket0 = CASE 
	                    WHEN (artb_temp.invoice_date between DATEADD(day,0,GetDate()) and DATEADD(day,0,GetDate()) )
		                    THEN
			                    artb_temp.total_amount
	                    ELSE
		                    0
	                    END,
             bucket1 = CASE 
	                    WHEN (artb_temp.invoice_date between DATEADD(day, -30,GetDate()) and DATEADD(day,0,GetDate()) )
		                    THEN
			                    artb_temp.total_amount
	                    ELSE
		                    0
	                    END,
	                    bucket2 = CASE 
	                    WHEN (artb_temp.invoice_date between DATEADD(day,-60,GetDATE()) and DATEADD(day,-31,GetDate()) )
		                    THEN
			                    artb_temp.total_amount
	                    ELSE
		                    0
	                    END,
	                    bucket3 = CASE 
	                    WHEN (artb_temp.invoice_date between DATEADD(day,-90,GetDATE()) and DATEADD(day,-61,GetDate()) )
		                    THEN
			                    artb_temp.total_amount
	                    ELSE
		                    0
	                    END,
	                    bucket4 = CASE 
	                    WHEN (artb_temp.invoice_date < DATEADD(day,-90,GetDATE()))
		                    THEN
			                    artb_temp.total_amount
	                    ELSE
		                    0
	                    END,
             total_due = artb_temp.total_amount,
             branch.branch_description,
             artb_temp.po_no,
             artb_temp.invoice_class,
             address_b.corp_address_id,
             customer.generate_statements_by,
             address_c.name,
             address_c.mail_address1,
             NULLIF(address_c.mail_address2, ''),
             ISNULL(address_c.mail_city, ''),
             ISNULL(address_c.mail_state, ''),
             ISNULL(address_c.mail_postal_code, ''),
             address_c.central_phone_number,
             customer.print_zero_dollar_customers,
             CAST({ ts '2019-03-19 06:12:06.552'}
                        AS datetime) statement_date,
             'N' group_by_credits,
             NULLIF(address_a.mail_country, ''),
             NULLIF(address_b.mail_country, ''),
             NULLIF(address_c.mail_country, ''),
             currency_hdr.currency_desc,
             CASE artb_temp.record_type_cd
                WHEN 2594
                    THEN artb_temp.record_type_reference_no
                ELSE ''
             END original_rebilled_invoice_no,
             artb_temp.tax_terms_taken_amt,
             NULLIF(address_a.mail_address3, '') ,
             NULLIF(address_b.mail_address3, '') ,
             NULLIF(address_c.mail_address3, '')
             FROM p21_fnt_artb(@year_period, 'MDS', 'Y', '0', '99999999', 0, GetDate(), 'N', @customer_id, @customer_id) artb_temp
             INNER JOIN company ON(company.company_id = artb_temp.company_id)
             INNER JOIN branch ON(branch.company_id = artb_temp.company_id) AND(branch.branch_id = artb_temp.branch_id)
             INNER JOIN customer ON(customer.company_id = artb_temp.company_id) AND(customer.customer_id = artb_temp.customer_id)
             INNER JOIN address AS address_a ON(address_a.id = company.address_id)
             INNER JOIN address AS address_b ON(address_b.id = artb_temp.customer_id)
             INNER JOIN address AS address_c ON(address_b.corp_address_id = address_c.id)
             INNER JOIN invoice_batch ON invoice_batch.invoice_batch_uid = artb_temp.statement_batch_uid
             LEFT JOIN currency_hdr ON currency_hdr.currency_id = customer.currency_id
             LEFT JOIN currency_line ON currency_line.currency_line_uid = artb_temp.currency_line_uid
             WHERE
                (customer.open_item_balance_forward = 'O') AND
               (artb_temp.total_receipts<>((total_amount + memo_amount + bad_debt_amount) - allowed_amount - terms_amount - tax_terms_taken_amt)) AND
                customer.generate_customer_statements = 'Y'AND
                invoice_batch.invoice_batch_number BETWEEN 0 AND 9999 AND
                (NULL IS NULL  OR currency_line.to_currency_id IS NULL OR COALESCE(currency_line.to_currency_id, 1) = 1) AND
               ((artb_temp.branch_id BETWEEN '0' AND 'ZZZZZZZZ') OR
              (artb_temp.branch_id IS NULL)) AND
                    artb_temp.date_created BETWEEN { ts '1990-01-01 00:00:00.000'}
                        AND { ts '2049-12-31 00:00:00.000'}
                        AND
                    artb_temp.date_last_modified BETWEEN { ts '1990-01-01 00:00:00.000'}
                        AND { ts '2049-12-31 00:00:00.000'}
                        AND artb_temp.last_maintained_by BETWEEN ' ' AND 'ZZZZZZZ'
                    AND artb_temp.approved = 'Y'
                    AND('A' = 'A' OR('G' = 'A' AND 'DD' = 'ID' AND GetDate() >= DATEADD(day, 0, artb_temp.invoice_date)) OR('G' = 'A' AND 'DD' = 'DD' AND { ts '2019-03-19 06:12:06.552'} >= DATEADD(day, 0, artb_temp.net_due_date)))
                    AND('Y' = 'Y' OR('N' = 'Y' AND artb_temp.invoice_adjustment_type <> 'C'))
                    AND('Y' = 'Y' OR('N' = 'Y' AND artb_temp.invoice_class <> 'Z'))
            /* Print all invoices if customers have at least one past due or past invoice date */
                    AND(('N' = 'N') OR(artb_temp.customer_id IN(SELECT artb_past_date.customer_id
                                                                        FROM p21_fnt_artb(@year_period, 'MDS', 'Y', '0', '99999999', 0, GetDate(), 'N', @customer_id, @customer_id) artb_past_date
                                                                    WHERE(artb_past_date.total_receipts<>((artb_past_date.total_amount
                                                                            + artb_past_date.memo_amount
                                                                            + artb_past_date.bad_debt_amount)
                                                                            - artb_past_date.allowed_amount
                                                                            - artb_past_date.terms_amount))
                    AND(artb_past_date.approved = 'Y') AND artb_past_date.customer_id = artb_temp.customer_id
                    AND((('DD' = 'ID') AND GetDate() >= Dateadd(day, 0, artb_past_date.invoice_date))
                    OR(('DD' = 'DD') AND GetDate() >= Dateadd(day, 0, artb_past_date.net_due_date) ))
            GROUP BY artb_past_date.customer_id) )) order by artb_temp.invoice_date	";
            #endregion
            using (var conn = DbConn.GetP21DbConnection())
            {
                try
                {
                    conn.Open();
                    return conn.Query<GetAgingReport>(sql, new
                    {

                        customer_id,
                        year_period

                    }).ToList();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return null;
        }

        public virtual IEnumerable<OrderPickTicket> GetPickTicketByOrderNo(string order_no)
        {
            string sql = @"
select	p.pick_ticket_no	
        ,p.company_id
        ,p.order_no
		,p.carrier_id
		,carrier_name = coalesce(csr.carrier_name, a.name)
		,tracking_no = coalesce(csr.tracking_no, sm.shipment_id, p.tracking_no)		     
from	oe_pick_ticket p
		left join clippership_return_10004 csr on csr.pick_ticket_no = p.pick_ticket_no
			and csr.delete_flag = 'N'
		left join shipment sm on p.pick_ticket_no = sm.transaction_no
			and sm.row_status_flag = 2175 -- accepted
			and sm.transaction_type_cd = 1000 -- pick ticket
		left join [address] a on a.id = coalesce(p.carrier_id, sm.carrier_id)
where	p.delete_flag <> 'Y'	
		and p.order_no = @order_no
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    return conn.Query<OrderPickTicket>(sql, new { order_no }).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString(), order_no);
            }
            return null;
        }

        public virtual IEnumerable<InvoiceShipment> GetInvoiceByDateLastModified(string company_id,
            DateTime? from_date, DateTime? to_date, IEnumerable<string> order_ids)
        {
            string sql = @"
select	oh.company_id
		,p21_order_id = oh.order_no
		,web_reference_id = oh.web_reference_no
		,p21_invoice_no = ih.invoice_no
		,il.item_id
		,qty = il.qty_shipped
		,il.unit_price
		,oh.delete_flag
		,cancel_flag = isnull(oh.cancel_flag, 'N')
		,approved = isnull(oh.approved, 'N')
		,completed = isnull(oh.completed, 'N')
        ,order_type = case when oh.projected_order = 'Y' then 'quote' else 'order' end
from	oe_hdr oh
        left join invoice_hdr ih on oh.order_no = ih.order_no
		left join invoice_line il on ih.invoice_no = il.invoice_no
where	oh.company_id = @company_id
        --and isnull(oh.projected_order, 'N') <> 'Y'
        and oh.source_code_no in (707, 931)
        and isnull(oh.rma_flag, 'N') <> 'Y'
        {{REPLACE}}
        {{REPLACE_INVOICE_DATE}}
		--and oh.web_reference_no is not null;

select	oh.company_id
		,p21_order_id = oh.order_no
		,web_reference_id = oh.web_reference_no
		,p.pick_ticket_no
		,carrier_code = p.carrier_id
		,title = coalesce(csr.carrier_name, a.name)
		,tracking_number = coalesce(csr.tracking_no, sm.shipment_id, p.tracking_no)		   
		,qty = d.ship_quantity
		,m.item_id  
		,oh.delete_flag
		,cancel_flag = isnull(oh.cancel_flag, 'N')
		,approved = isnull(oh.approved, 'N')
		,completed = isnull(oh.completed, 'N')
        ,order_type = case when oh.projected_order = 'Y' then 'quote' else 'order' end
from	oe_hdr oh
		left join oe_pick_ticket p on p.order_no = oh.order_no
			and p.company_id = oh.company_id
		left join oe_pick_ticket_detail d on p.company_id = d.company_id
			and p.pick_ticket_no = d.pick_ticket_no
		left join inv_mast m on d.inv_mast_uid = m.inv_mast_uid
		left join clippership_return_10004 csr on csr.pick_ticket_no = p.pick_ticket_no
			and csr.delete_flag = 'N'
		left join shipment sm on p.pick_ticket_no = sm.transaction_no
			and sm.row_status_flag = 2175 -- accepted
			and sm.transaction_type_cd = 1000 -- pick ticket
		left join [address] a on a.id = coalesce(p.carrier_id, sm.carrier_id)
where	oh.company_id = @company_id
        and oh.source_code_no in (707, 931)
        and isnull(oh.rma_flag, 'N') <> 'Y'
        {{REPLACE}}
        {{REPLACE_SHIPMENT_DATE}}
		--and oh.web_reference_no is not null;
";
            try
            {
                if (order_ids != null && order_ids.Any())
                    sql = sql.Replace("{{REPLACE}}", "and oh.web_reference_no in @order_ids");
                else
                    sql = sql.Replace("{{REPLACE}}", "");

                // from and to date logic
                if (from_date.HasValue && !to_date.HasValue)
                {
                    sql = sql.Replace("{{REPLACE_INVOICE_DATE}}", @"
		and
		(
			oh.date_last_modified >= @from
			or 
			(
				ih.date_last_modified is not null
				and ih.date_last_modified >= @from
			)
		)
");
                    sql = sql.Replace("{{REPLACE_SHIPMENT_DATE}}", @"
		and
		(
			oh.date_last_modified >= @from
			or
			(
				p.date_last_modified is not null
				and p.date_last_modified >= @from
			)
			or
			(
				csr.date_last_modified is not null
				and csr.date_last_modified >= @from
			)
		)
");
                }
                else if (from_date.HasValue && to_date.HasValue)
                {
                    sql = sql.Replace("{{REPLACE_INVOICE_DATE}}", @"
		and 
		(
			oh.date_last_modified between @from and @to
			or
			(
				ih.date_last_modified is not null
				and ih.date_last_modified between @from and @to
			)
		)
");
                    sql = sql.Replace("{{REPLACE_SHIPMENT_DATE}}", @"
and 
		(
			oh.date_last_modified between @from and @to
			or
			(
				p.date_last_modified is not null
				and p.date_last_modified between @from and @to
			)
			or
			(
				csr.date_last_modified is not null
				and csr.date_last_modified between @from and @to
			)
		)
");
                }
                else
                {
                    sql = sql.Replace("{{REPLACE_INVOICE_DATE}}", "");
                    sql = sql.Replace("{{REPLACE_SHIPMENT_DATE}}", "");
                }

                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    dynamic param = new { company_id };
                    if (order_ids != null && order_ids.Any())
                        param = new { company_id, order_ids };
                    if (from_date.HasValue && to_date.HasValue)
                        param = new { company_id, from = from_date, to = to_date };
                    else if (from_date.HasValue && !to_date.HasValue)
                        param = new { company_id, from = from_date };


                    using (var multi = conn.QueryMultiple(sql, (object)param))
                    {
                        var invoices = multi.Read<InvoiceFlat>()
                            .ToList()
                            .GroupBy(t => new
                            {
                                t.company_id,
                                t.p21_order_id,
                                t.web_reference_id,
                                t.delete_flag,
                                t.cancel_flag,
                                t.approved,
                                t.completed,
                                t.order_type
                            }).ToList();


                        var shipments = multi.Read<ShipmentFlat>()
                            .ToList()
                            .GroupBy(t => new
                            {
                                t.company_id,
                                t.p21_order_id,
                                t.web_reference_id,
                                t.delete_flag,
                                t.cancel_flag,
                                t.approved,
                                t.completed,
                                t.order_type
                            }).ToList();

                        var results = new List<InvoiceShipment>();

                        foreach (var invoice in invoices)
                        {
                            var order = new InvoiceShipment()
                            {
                                company_id = invoice.Key.company_id,
                                p21_order_id = invoice.Key.p21_order_id,
                                web_reference_id = invoice.Key.web_reference_id,
                                delete_flag = invoice.Key.delete_flag,
                                cancel_flag = invoice.Key.cancel_flag,
                                approved = invoice.Key.approved,
                                completed = invoice.Key.completed,
                                order_type = invoice.Key.order_type
                            };

                            order.invoices = invoice
                                .Where(t => !string.IsNullOrEmpty(t.p21_invoice_no))
                                .GroupBy(t => t.p21_invoice_no)
                                .Select(t => new Invoice()
                                {
                                    p21_invoice_no = t.Key,
                                    items = t.Where(_ => !string.IsNullOrEmpty(_.item_id))
                                        .Select(_ => new InvoiceLine()
                                        {
                                            item_id = _.item_id,
                                            qty = _.qty ?? 0,
                                            unit_price = _.unit_price ?? 0
                                        }).ToList()
                                }).ToList();

                            results.Add(order);
                        }

                        foreach (var shipment in shipments)
                        {
                            var order = results.FirstOrDefault(t => t.p21_order_id == shipment.Key.p21_order_id);
                            var found = order != null;

                            if (!found)
                            {
                                order = new InvoiceShipment()
                                {
                                    company_id = shipment.Key.company_id,
                                    p21_order_id = shipment.Key.p21_order_id,
                                    web_reference_id = shipment.Key.web_reference_id,
                                    delete_flag = shipment.Key.delete_flag,
                                    cancel_flag = shipment.Key.cancel_flag,
                                    approved = shipment.Key.approved,
                                    completed = shipment.Key.completed,
                                    order_type = shipment.Key.order_type
                                };
                            }

                            order.shipments = shipment
                                .Where(t => t.pick_ticket_no.HasValue)
                                .GroupBy(t => new
                                {
                                    t.pick_ticket_no,
                                    t.carrier_code,
                                    t.title,
                                    t.tracking_number
                                })
                                .Select(t => new Shipment()
                                {
                                    pick_ticket_no = t.Key.pick_ticket_no,
                                    carrier_code = t.Key.carrier_code,
                                    title = t.Key.title,
                                    tracking_number = t.Key.tracking_number,
                                    items = t.Where(_ => !string.IsNullOrEmpty(_.item_id))
                                        .Select(_ => new ShipmentLine()
                                        {
                                            item_id = _.item_id,
                                            qty = _.qty ?? 0
                                        }).ToList()
                                }).ToList();

                            if (!found)
                                results.Add(order);
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString(), company_id, from_date, to_date, order_ids ?? new List<string>());
            }
            return null;
        }

        public virtual IEnumerable<Order> Find(string company_id,
            DateTime? from_date, DateTime? to_date, IEnumerable<string> web_ref_numbers)
        {
            #region sql
            string sql = @"
select	oh.company_id
		,oh.order_no
from	oe_hdr oh
		left join oe_line ol on oh.order_no = ol.order_no
		left join 
		(
			select	ih.order_no
					,ih.company_no
			from	invoice_hdr ih
					join invoice_line il on ih.invoice_no = il.invoice_no
			where	ih.date_last_modified between @from and @to
					or il.date_last_modified between @from and @to
			group by ih.order_no
					,ih.company_no
		) i on oh.order_no = i.order_no
			and oh.company_id = i.company_no
		left join
		(
			select	p.order_no
					,p.company_id
			from	oe_pick_ticket p
					left join oe_pick_ticket_detail d on p.company_id = d.company_id
						and p.pick_ticket_no = d.pick_ticket_no
					left join clippership_return_10004 csr on csr.pick_ticket_no = p.pick_ticket_no
						and csr.delete_flag = 'N'
					left join shipment sm on p.pick_ticket_no = sm.transaction_no
						and sm.row_status_flag = 2175 -- accepted
						and sm.transaction_type_cd = 1000 -- pick ticket
			where	p.date_last_modified between @from and @to
					or d.date_last_modified between @from and @to
					or csr.date_last_modified between @from and @to
					or sm.date_last_modified between @from and @to
		) s on oh.order_no = s.order_no
			and oh.company_id = s.company_id
where	oh.company_id = @company_id
        and oh.source_code_no in (707, 931, 3067)
        and isnull(oh.rma_flag, 'N') <> 'Y'
		and
		(
			oh.date_last_modified between @from and @to
			or ol.date_last_modified between @from and @to
		)
        {{REPLACE}}
group by oh.company_id
		,oh.order_no
";
            #endregion

            if (!from_date.HasValue) from_date = DateTime.Now.AddYears(-100);
            if (!to_date.HasValue) to_date = DateTime.Now.AddYears(1);
            if (web_ref_numbers != null && web_ref_numbers.Any())
                sql = sql.Replace("{{REPLACE}}", "and oh.web_reference_no in @web_ref_numbers");
            else
                sql = sql.Replace("{{REPLACE}}", "");

            var results = new List<Order>();

            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    dynamic param = new { company_id, from = from_date, to = to_date };
                    if (web_ref_numbers != null && web_ref_numbers.Any())
                        param = new { company_id, from = from_date, to = to_date, web_ref_numbers };

                    var data = conn.Query<OrderKey>(sql, (object)param).ToList();
                    if (data == null) return null;

                    foreach (var d in data)
                    {
                        var o = Get(d.company_id, d.order_no, conn);
                        if (o != null) results.Add(o);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            return results;
        }
        public virtual IEnumerable<SalesForceOrder> SalesForceFind(string company_id,
            DateTime? from_date, DateTime? to_date, IEnumerable<string> web_ref_numbers, int page_number = 1, int page_size = 15, int line_count = -1)

        {
            #region sql
            string sql = @"
;with orders as(select	oh.company_id
		,oh.order_no
        ,order_line_count = (select Max(line_no) from oe_line where oe_line.order_no = oh.order_no)
from	oe_hdr oh
		left join oe_line ol on oh.order_no = ol.order_no
		left join 
		(
			select	ih.order_no
					,ih.company_no
			from	invoice_hdr ih
					join invoice_line il on ih.invoice_no = il.invoice_no
			where	ih.date_last_modified between @from and @to
					or il.date_last_modified between @from and @to
			group by ih.order_no
					,ih.company_no
		) i on oh.order_no = i.order_no
			and oh.company_id = i.company_no
		left join
		(
			select	p.order_no
					,p.company_id
			from	oe_pick_ticket p
					left join oe_pick_ticket_detail d on p.company_id = d.company_id
						and p.pick_ticket_no = d.pick_ticket_no
					left join clippership_return_10004 csr on csr.pick_ticket_no = p.pick_ticket_no
						and csr.delete_flag = 'N'
					left join shipment sm on p.pick_ticket_no = sm.transaction_no
						and sm.row_status_flag = 2175 -- accepted
						and sm.transaction_type_cd = 1000 -- pick ticket
			where	p.date_last_modified between @from and @to
					or d.date_last_modified between @from and @to
					or csr.date_last_modified between @from and @to
					or sm.date_last_modified between @from and @to
		) s on oh.order_no = s.order_no
			and oh.company_id = s.company_id
where	oh.company_id = @company_id
        and oh.source_code_no in (706, 707, 931, 3067)
        and isnull(oh.rma_flag, 'N') <> 'Y'
		and
		(
			oh.date_last_modified between @from and @to
			or ol.date_last_modified between @from and @to
		)
        {{REPLACE}}
group by oh.company_id
		,oh.order_no
)
select 
      *,Total = cnt
from orders
                cross join (select cnt = count(order_no) from orders where order_line_count <= case when @line_count <> -1 Then @line_count Else order_line_count end) as cnt
                where order_line_count <= case when @line_count <> -1 Then @line_count Else order_line_count end
                order by order_no
                OFFSET ((@page_number-1) * @page_size) ROWS
                FETCH NEXT @page_size ROWS ONLY
";
            #endregion

            if (!from_date.HasValue) from_date = DateTime.UtcNow.AddYears(-100);
            if (!to_date.HasValue) to_date = DateTime.UtcNow.AddYears(1);
            if (web_ref_numbers != null && web_ref_numbers.Any())
                sql = sql.Replace("{{REPLACE}}", "and oh.web_reference_no in @web_ref_numbers");
            else
                sql = sql.Replace("{{REPLACE}}", "");

            var results = new List<SalesForceOrder>();

            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    dynamic param = new { company_id, from = from_date, to = to_date, page_number, page_size, line_count };
                    if (web_ref_numbers != null && web_ref_numbers.Any())
                        param = new { company_id, from = from_date, to = to_date, web_ref_numbers, page_size, page_number, line_count };

                    var data = conn.Query<SalesForceOrderKey>(sql, (object)param).ToList();
                    Logger.LogDebug($"Count {data.Count}");
                    if (data == null) return null;
                    int index = 1;
                    Parallel.ForEach<SalesForceOrderKey>(data, (d) => {
                        Logger.LogDebug("Index : " + index + "order_no : " + d.order_no + d.order_line_count);
                        var o = SalesForceGet(d.company_id, d.order_no, conn);
                        if (o != null)
                        {
                            o.Total = d.Total;
                            results.Add(o);
                        }
                        index++;
                    });
                    //foreach (var d in data)
                    //{
                    //    Logger.LogDebug("Index : "+index);
                    //    var o = Get(d.company_id, d.order_no, conn);
                    //    if (o != null)
                    //    {
                    //        o.Total = d.Total;
                    //        results.Add(o);
                    //    }
                    //    index++;
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            return results;
        }

        protected Order Get(string company_id, string order_no, IDbConnection conn)
        {
            #region sql
            string sql = @"
-- order hdr
select	oh.company_id
		,p21_order_id = oh.order_no
		,web_reference_id = oh.web_reference_no
		,oh.delete_flag
		,cancel_flag = isnull(oh.cancel_flag, 'N')
		,approved = isnull(oh.approved, 'N')
		,completed = isnull(oh.completed, 'N')
        ,order_type = case when oh.projected_order = 'Y' then 'quote' else 'order' end
from	oe_hdr oh
where	oh.company_id = @company_id
        and oh.order_no = @order_no

-- order line
select	qty = (l.qty_ordered - l.qty_canceled) / l.unit_size
		,m.item_id
		,l.unit_price
        ,l.unit_quantity
from	oe_line l
		join inv_mast m on l.inv_mast_uid = m.inv_mast_uid
where	l.order_no = @order_no
		and l.company_no = @company_id

-- invoice
select	p21_invoice_no = ih.invoice_no
from	invoice_hdr ih
where	ih.company_no = @company_id
        and ih.order_no = @order_no

-- invoice line
select	p21_invoice_no = il.invoice_no
		,qty = il.qty_shipped / isnull(il.sales_unit_size, 1)
		,item_id = m.item_id
		,il.unit_price
from	invoice_line il
		join inv_mast m on il.inv_mast_uid = m.inv_mast_uid
where	il.company_id = @company_id
		and il.order_no = @order_no

-- shipment
select	p.pick_ticket_no
		,carrier_code = p.carrier_id
		,title = coalesce(csr.carrier_name, a.name)
		,tracking_number = coalesce(csr.tracking_no, sm.shipment_id, p.tracking_no)		   
from	oe_pick_ticket p
		left join clippership_return_10004 csr on csr.pick_ticket_no = p.pick_ticket_no
			and csr.delete_flag = 'N'
		left join shipment sm on p.pick_ticket_no = sm.transaction_no
			and sm.row_status_flag = 2175 -- accepted
			and sm.transaction_type_cd = 1000 -- pick ticket
		left join [address] a on a.id = coalesce(p.carrier_id, sm.carrier_id)
where	p.company_id = @company_id
		and p.order_no = @order_no
		and p.delete_flag <> 'Y'
		and isnull(csr.delete_flag, 'N') <> 'Y'

-- shipment line
select	qty = d.ship_quantity / d.unit_size
		,m.item_id  
		,p.pick_ticket_no
from	oe_pick_ticket p
		join oe_pick_ticket_detail d on p.company_id = d.company_id
			and p.pick_ticket_no = d.pick_ticket_no
		left join inv_mast m on d.inv_mast_uid = m.inv_mast_uid
where	p.company_id = @company_id
		and p.order_no = @order_no
		and p.delete_flag <> 'Y'
";
            #endregion

            try
            {
                using (var multi = conn.QueryMultiple(sql, new { company_id, order_no }))
                {
                    var order = multi.Read<Order>().FirstOrDefault();
                    if (order == null) return null;
                    order.lines = multi.Read<OrderLine>().ToList();
                    order.invoices = multi.Read<OrderInvoice>().ToList();
                    var invoiceLines = multi.Read<OrderInvoiceLine>().ToList();
                    foreach (var invoice in order.invoices)
                    {
                        invoice.items = invoiceLines
                            .Where(_ => _.p21_invoice_no == invoice.p21_invoice_no)
                            .ToList();
                    }
                    order.shipments = multi.Read<OrderShipment>().ToList();
                    var shipmentLines = multi.Read<OrderShipmentLine>().ToList();
                    foreach (var shipment in order.shipments)
                    {
                        shipment.items = shipmentLines
                            .Where(_ => _.pick_ticket_no == shipment.pick_ticket_no)
                            .ToList();
                    }

                    return order;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
            return null;
        }
        protected SalesForceOrder SalesForceGet(string company_id, string order_no, IDbConnection conn)
        {
            {
                Logger.LogDebug($"Info :{company_id},{order_no}");
                #region sql
                string sql = @"
-- order hdr
select	oeh_order_no = oh.order_no,
oeh_customer_id = oh.customer_id,
oeh_order_date = oh.order_date,
oeh_ship2_name = oh.ship2_name,
oeh_ship2_add1 = oh.ship2_add1,
oeh_ship2_add2 = oh.ship2_add2,
oeh_ship2_city = oh.ship2_city,
oeh_ship2_state = oh.ship2_state,
oeh_ship2_zip = oh.ship2_zip,
oeh_ship2_country = oh.ship2_country,
oeh_requested_date = oh.requested_date,
oeh_po_no = oh.po_no,
oeh_terms = oh.terms,
oeh_ship_to_phone = oh.ship_to_phone,
oeh_delete_flag = oh.delete_flag,
oeh_completed = oh.completed,
oeh_company_id = oh.company_id,
oeh_date_created = oh.date_created,
oeh_date_last_modified = oh.date_last_modified,
oeh_last_maintained_by = oh.last_maintained_by,
oeh_cod_flag = oh.cod_flag,
oeh_po_no_append = oh.po_no_append,
oeh_location_id = oh.location_id,
oeh_carrier_id = oh.carrier_id,
oeh_address_id = oh.address_id,
oeh_contact_id = oh.contact_id,
oeh_corp_address_id = oh.corp_address_id,
oeh_handling_charge_req_flag = oh.handling_charge_req_flag,
oeh_payment_method = oh.payment_method,
oeh_fob_flag = oh.fob_flag,
oeh_class_1id = oh.class_1id,
oeh_class_2id = oh.class_2id,
oeh_class_3id = oh.class_3id,
oeh_class_4id = oh.class_4id,
oeh_class_5id = oh.class_5id,
oeh_rma_flag = oh.rma_flag,
oeh_taker = oh.taker,
oeh_third_party_billing_flag = oh.third_party_billing_flag,
oeh_approved = oh.approved,
oeh_source_location_id = oh.source_location_id,
oeh_packing_basis = oh.packing_basis,
oeh_delivery_instructions = oh.delivery_instructions,
oeh_pick_ticket_type = oh.pick_ticket_type,
oeh_requested_downpayment = oh.requested_downpayment,
oeh_downpayment_invoiced = oh.downpayment_invoiced,
oeh_cancel_flag = oh.cancel_flag,
oeh_will_call = oh.will_call,
oeh_front_counter = oh.front_counter,
oeh_validation_status = oh.validation_status,
oeh_oe_hdr_uid = oh.oe_hdr_uid,
oeh_credit_card_hold = oh.credit_card_hold,
oeh_freight_out = oh.freight_out,
oeh_exclude_rebates = oh.exclude_rebates,
oeh_job_price_hdr_uid = oh.job_price_hdr_uid,
oeh_front_counter_rma = oh.front_counter_rma,
oeh_ship2_email_address = oh.ship2_email_address,
oeh_order_type = oh.order_type,
oeh_taxable = oh.taxable,
oeh_profit_percent = oh.profit_percent,
oeh_invoice_exch_rate_source_cd = oh.invoice_exch_rate_source_cd,
oeh_rma_expiration_date = oh.rma_expiration_date,
oeh_created_by = oh.created_by,
oeh_invoice_no = oh.invoice_no,
oeh_bill_to_contact_id = oh.bill_to_contact_id,
oeh_tag_hold_cancel_date = oh.tag_hold_cancel_date,
oeh_restock_fee_percentage = oh.restock_fee_percentage,
oeh_date_order_completed = oh.date_order_completed,
oeh_validated_via_open_orders_flag = oh.validated_via_open_orders_flag,
oeh_original_promise_date = oh.original_promise_date,
oeh_promise_date = oh.promise_date,
oeh_requested_ship_date = oh.requested_ship_date,
oeh_expected_completion_date = oh.expected_completion_date,
oeh_job_control_flag = oh.job_control_flag,
oeh_merchandise_credit_flag = oh.merchandise_credit_flag,
oeh_req_pymt_upon_release_flag = oh.req_pymt_upon_release_flag,
oeh_downpayment_percentage = oh.downpayment_percentage,
oeh_prepaid_invoice_flag = oh.prepaid_invoice_flag,
oeh_exported_flag = oh.exported_flag,
oeh_web_reference_no = oh.web_reference_no,
oeh_ups_code = oh.ups_code,
oeh_default_pricing_cd = oh.default_pricing_cd,
oeh_expedite_date = oh.expedite_date,
oeh_environmental_fee_flag = oh.environmental_fee_flag,
oeh_admin_fee_flag = oh.admin_fee_flag,
oeh_promise_date_extended_desc = oh.promise_date_extended_desc,
oeh_promise_date_edited_date = oh.promise_date_edited_date,
oeh_send_partial_order_flag = oh.send_partial_order_flag,
oeh_b2b_ups_freight_amount = oh.b2b_ups_freight_amount,
oeh_user_defined_date = oh.user_defined_date,
oeh_replace_company_name_flag = oh.replace_company_name_flag,
oeh_first_packing_list_no = oh.first_packing_list_no,
oeh_ship_confirmed_flag = oh.ship_confirmed_flag,
oeh_price_confirmed_flag = oh.price_confirmed_flag,
oeh_quoted_freight_out = oh.quoted_freight_out,
oeh_geocom_append = oh.geocom_append,
oeh_apply_fuel_surcharge_flag = oh.apply_fuel_surcharge_flag,
--oeh_WebOrderID = oh.WebOrderID,
oeh_override_min_order_charge_flag = oh.override_min_order_charge_flag,
oeh_approved_for_ar_flag = oh.approved_for_ar_flag,
oeh_subject = oh.subject,
oeh_url = oh.url,
oeh_original_packing_basis = oh.original_packing_basis,
oeh_freight_out_edited_flag = oh.freight_out_edited_flag,
oeh_override_contact_email_flag = oh.override_contact_email_flag,
oeh_override_email_address = oh.override_email_address,
oeh_print_prices_on_packinglist = oh.print_prices_on_packinglist,
oeh_web_shopper_id = oh.web_shopper_id,
oeh_web_shopper_email = oh.web_shopper_email,
oeh_limit_max_shipments_per_order = oh.limit_max_shipments_per_order,
oeh_ship2_add3 = oh.ship2_add3,
oeh_bill_to_id = oh.bill_to_id,
oeh_electronic_order_flag = oh.electronic_order_flag,
oeh_hold_invoice_flag = oh.hold_invoice_flag,
oeh_salesrep_id = os.salesrep_id
from	oe_hdr oh
left join oe_hdr_salesrep os on os.order_number = oh.order_no
where	oh.company_id = @company_id
        and oh.order_no = @order_no

-- order line
select	oel_qty = (l.qty_ordered - l.qty_canceled) / l.unit_size
		,oel_item_id = m.item_id
        ,oel_item_desc = m.item_desc
		,oel_order_no = l.order_no,
oel_unit_price = l.unit_price,
oel_qty_ordered = l.qty_ordered,
oel_qty_per_assembly = l.qty_per_assembly,
oel_company_no = l.company_no,
oel_delete_flag = l.delete_flag,
oel_manual_price_overide = l.manual_price_overide,
oel_date_created = l.date_created,
oel_date_last_modified = l.date_last_modified,
oel_last_maintained_by = l.last_maintained_by,
oel_extended_price = l.extended_price,
oel_sales_tax = l.sales_tax,
oel_line_no = l.line_no,
oel_complete = l.complete,
oel_unit_of_measure = l.unit_of_measure,
oel_base_ut_price = l.base_ut_price,
oel_calc_type = l.calc_type,
oel_calc_value = l.calc_value,
oel_combinable = l.combinable,
oel_disposition = l.disposition,
oel_qty_allocated = l.qty_allocated,
oel_qty_on_pick_tickets = l.qty_on_pick_tickets,
oel_qty_invoiced = l.qty_invoiced,
oel_expedite_date = l.expedite_date,
oel_required_date = l.required_date,
oel_source_loc_id = l.source_loc_id,
oel_ship_loc_id = l.ship_loc_id,
oel_supplier_id = l.supplier_id,
oel_product_group_id = l.product_group_id,
oel_assembly = l.assembly,
oel_scheduled = l.scheduled,
oel_lot_bill = l.lot_bill,
oel_extended_desc = l.extended_desc,
oel_unit_size = l.unit_size,
oel_unit_quantity = l.unit_quantity,
oel_next_break = l.next_break,
oel_next_ut_price = l.next_ut_price,
oel_customer_part_number = l.customer_part_number,
oel_tax_item = l.tax_item,
oel_ok_to_interchange = l.ok_to_interchange,
oel_other_charge = l.other_charge,
oel_sales_cost = l.sales_cost,
oel_commission_cost = l.commission_cost,
oel_requested_downpayment = l.requested_downpayment,
oel_downpayment_date = l.downpayment_date,
oel_will_call = l.will_call,
oel_other_cost = l.other_cost,
oel_downpayment_remaining = l.downpayment_remaining,
oel_cancel_flag = l.cancel_flag,
oel_commission_cost_edited = l.commission_cost_edited,
oel_other_cost_edited = l.other_cost_edited,
oel_po_cost = l.po_cost,
oel_qty_canceled = l.qty_canceled,
oel_pricing_unit = l.pricing_unit,
oel_pricing_unit_size = l.pricing_unit_size,
oel_qty_staged = l.qty_staged,
oel_item_source = l.item_source,
oel_oe_hdr_uid = l.oe_hdr_uid,
oel_parent_oe_line_uid = l.parent_oe_line_uid,
oel_oe_line_uid = l.oe_line_uid,
oel_price_page_uid = l.price_page_uid,
oel_pricing_option = l.pricing_option,
oel_detail_type = l.detail_type,
oel_user_line_no = l.user_line_no,
oel_item_terms_discount_pct = l.item_terms_discount_pct,
oel_default_in_oe = l.default_in_oe,
oel_inv_mast_uid = l.inv_mast_uid,
oel_freight_in = l.freight_in,
oel_substitute_item = l.substitute_item,
oel_allocate_usage_to_original_item = l.allocate_usage_to_original_item,
oel_order_cost_edited = l.order_cost_edited,
oel_pick_date = l.pick_date,
oel_created_by = l.created_by,
oel_original_qty_ordered = l.original_qty_ordered,
oel_disposition_edited_flag = l.disposition_edited_flag,
oel_restock_fee_percentage = l.restock_fee_percentage,
oel_customer_configured_price = l.customer_configured_price,
oel_customer_picked_flag = l.customer_picked_flag,
oel_job_price_line_uid = l.job_price_line_uid,
oel_cust_po_no = l.cust_po_no,
oel_use_contract_cost = l.use_contract_cost,
oel_cost_price_page_uid = l.cost_price_page_uid,
oel_ud_cost = l.ud_cost,
oel_item_bill_to_id = l.item_bill_to_id,
oel_buyback_no = l.buyback_no,
oel_additional_description = l.additional_description,
oel_bill_hold_flag = l.bill_hold_flag,
oel_clock = l.clock,
oel_cell = l.cell,
oel_environmental_fee = l.environmental_fee,
oel_admin_fee = l.admin_fee,
oel_core_price = l.core_price,
oel_ext_disc_amt = l.ext_disc_amt,
oel_system_calc_unit_price = l.system_calc_unit_price,
oel_buyer = l.buyer,
oel_recipient = l.recipient,
oel_original_unit_price = l.original_unit_price,
oel_quote_price_oe_line_uid = l.quote_price_oe_line_uid,
oel_price1 = l.price1,
oel_retail_price = l.retail_price,
oel_sales_discount_group_id = l.sales_discount_group_id,
oel_price_family_uid = l.price_family_uid,
oel_competitor_uid = l.competitor_uid,
oel_price_adj_note = l.price_adj_note,
oel_rfq_indicator_flag = l.rfq_indicator_flag,
oel_buy_get_rewards_flag = l.buy_get_rewards_flag,
oel_buy_get_x_rewards_program_uid = l.buy_get_x_rewards_program_uid,
oel_carrier_rebate_cost = l.carrier_rebate_cost,
oel_line_discount = l.line_discount,
oel_line_discount_description = l.line_discount_description,
oel_extended_desc_location = l.extended_desc_location,
oel_extended_desc_customer = l.extended_desc_customer,
oel_eco_fee_amount = l.eco_fee_amount,
oel_generic_custom_description = l.generic_custom_description,
oel_line_seq_no = l.line_seq_no,
oel_secondary_unit_price = l.secondary_unit_price,
oel_secondary_extended_price = l.secondary_extended_price,
oel_secondary_manual_price_overide = l.secondary_manual_price_overide,
oel_currentbin_uid = l.currentbin_uid,
oel_qa_status = l.qa_status,
oel_prediscount_unit_price = l.prediscount_unit_price,
oel_cust_percentage_disc = l.cust_percentage_disc,
oel_original_qty_allocated = l.original_qty_allocated
from	oe_line l
		join inv_mast m on l.inv_mast_uid = m.inv_mast_uid
where	l.order_no = @order_no
		and l.company_no = @company_id

-- invoice
select	ih_invoice_no = ih.invoice_no,
ih_order_no = ih.order_no,
ih_order_date = ih.order_date,
ih_invoice_date = ih.invoice_date,
ih_customer_id = ih.customer_id,
ih_bill2_name = ih.bill2_name,
ih_bill2_contact = ih.bill2_contact,
ih_bill2_address1 = ih.bill2_address1,
ih_bill2_address2 = ih.bill2_address2,
ih_bill2_city = ih.bill2_city,
ih_bill2_state = ih.bill2_state,
ih_bill2_postal_code = ih.bill2_postal_code,
ih_ship2_name = ih.ship2_name,
ih_ship2_contact = ih.ship2_contact,
ih_ship2_address1 = ih.ship2_address1,
ih_ship2_address2 = ih.ship2_address2,
ih_ship2_city = ih.ship2_city,
ih_ship2_state = ih.ship2_state,
ih_ship2_postal_code = ih.ship2_postal_code,
ih_carrier_name = ih.carrier_name,
ih_fob = ih.fob,
ih_terms_desc = ih.terms_desc,
ih_po_no = ih.po_no,
ih_salesrep_id = ih.salesrep_id,
ih_salesrep_name = ih.salesrep_name,
ih_brokerage = ih.brokerage,
ih_freight = ih.freight,
ih_ar_account_no = ih.ar_account_no,
ih_gl_freight_account_no = ih.gl_freight_account_no,
ih_gl_brokerage_account_no = ih.gl_brokerage_account_no,
ih_brokerage_amt = ih.brokerage_amt,
ih_period = ih.period,
ih_year_for_period = ih.year_for_period,
ih_store_no = ih.store_no,
ih_invoice_type = ih.invoice_type,
ih_ship_to_id = ih.ship_to_id,
ih_ship_date = ih.ship_date,
ih_total_amount = ih.total_amount,
ih_amount_paid = ih.amount_paid,
ih_terms_taken = ih.terms_taken,
ih_allowed = ih.allowed,
ih_paid_in_full_flag = ih.paid_in_full_flag,
ih_date_paid = ih.date_paid,
ih_print_flag = ih.print_flag,
ih_print_date = ih.print_date,
ih_company_no = ih.company_no,
ih_customer_id_number = ih.customer_id_number,
ih_date_created = ih.date_created,
ih_date_last_modified = ih.date_last_modified,
ih_last_maintained_by = ih.last_maintained_by,
ih_printed = ih.printed,
ih_printed_date = ih.printed_date,
ih_corp_address_id = ih.corp_address_id,
ih_shipping_cost = ih.shipping_cost,
ih_bill2_country = ih.bill2_country,
ih_ship2_country = ih.ship2_country,
ih_invoice_reference_no = ih.invoice_reference_no,
ih_invoice_desc = ih.invoice_desc,
ih_memo_amount = ih.memo_amount,
ih_bad_debt_amount = ih.bad_debt_amount,
ih_invoice_class = ih.invoice_class,
ih_period_fully_paid = ih.period_fully_paid,
ih_year_fully_paid = ih.year_fully_paid,
ih_approved = ih.approved,
ih_net_due_date = ih.net_due_date,
ih_terms_due_date = ih.terms_due_date,
ih_terms_id = ih.terms_id,
ih_branch_id = ih.branch_id,
ih_disputed_flag = ih.disputed_flag,
ih_other_charge_amount = ih.other_charge_amount,
ih_tax_amount = ih.tax_amount,
ih_sold_to_ah_uid = ih.sold_to_ah_uid,
ih_sold_to_customer_id = ih.sold_to_customer_id,
ih_ship_to_phone = ih.ship_to_phone,
ih_terms_amount = ih.terms_amount,
ih_sales_location_id = ih.sales_location_id,
ih_ship2_email_address = ih.ship2_email_address,
ih_created_by = ih.created_by,
ih_job_id = ih.job_id,
ih_invoice_paid_period = ih.invoice_paid_period,
ih_invoice_period = ih.invoice_period,
ih_commission_cost = ih.commission_cost,
ih_external_reference_no = ih.external_reference_no,
ih_total_freightcharge_weight = ih.total_freightcharge_weight,
ih_receiver_name = ih.receiver_name,
ih_downpayment_applied = ih.downpayment_applied,
--ih_update_tracking = ih.update_tracking,
ih_sent_to_carrier_date = ih.sent_to_carrier_date,
ih_tax_terms_amt = ih.tax_terms_amt,
ih_tax_amount_paid = ih.tax_amount_paid,
ih_bill2_address3 = ih.bill2_address3,
ih_ship2_address3 = ih.ship2_address3,
ih_iva_withheld_amount = ih.iva_withheld_amount,
ih_carrier_id = ih.carrier_id,
ih_free_out_freight_min_met_flag = ih.free_out_freight_min_met_flag,
ih_ship2_latitude = ih.ship2_latitude,
ih_ship2_longitude = ih.ship2_longitude,
ih_bill_to_supplier_flag = ih.bill_to_supplier_flag,
ih_actual_freight_cost = ih.actual_freight_cost
from	invoice_hdr ih
where	ih.company_no = @company_id
        and ih.order_no = @order_no

-- invoice line
select	
		il_qty = il.qty_shipped / isnull(il.sales_unit_size, 1)
		,il_item_id = m.item_id,
il_invoice_no = il.invoice_no,
il_qty_requested = il.qty_requested,
il_qty_shipped = il.qty_shipped,
il_unit_of_measure = il.unit_of_measure,
il_item_id = il.item_id,
il_item_desc = il.item_desc,
il_unit_price = il.unit_price,
il_extended_price = il.extended_price,
il_date_created = il.date_created,
il_date_last_modified = il.date_last_modified,
il_last_maintained_by = il.last_maintained_by,
il_order_no = il.order_no,
il_cogs_amount = il.cogs_amount,
il_job_id = il.job_id,
il_customer_part_number = il.customer_part_number,
il_company_id = il.company_id,
il_tax_item = il.tax_item,
il_pricing_quantity = il.pricing_quantity,
il_net_quantity = il.net_quantity,
il_line_no = il.line_no,
il_sales_cost = il.sales_cost,
il_commission_cost = il.commission_cost,
il_other_cost = il.other_cost,
il_oe_line_number = il.oe_line_number,
il_other_charge_item = il.other_charge_item,
il_exceptional_sales = il.exceptional_sales,
il_pricing_unit = il.pricing_unit,
il_invoice_line_uid = il.invoice_line_uid,
il_invoice_line_uid_parent = il.invoice_line_uid_parent,
il_inv_mast_uid = il.inv_mast_uid,
il_invoice_line_type = il.invoice_line_type,
il_product_group_id = il.product_group_id,
il_supplier_id = il.supplier_id,
il_created_by = il.created_by,
il_job_price_line_uid = il.job_price_line_uid,
il_customer_contract_uid = il.customer_contract_uid,
il_one_time_price_flag = il.one_time_price_flag,
il_environmental_fee = il.environmental_fee,
il_admin_fee = il.admin_fee,
il_core_price = il.core_price,
il_covered_extended_price = il.covered_extended_price,
il_cost_price_page_uid = il.cost_price_page_uid,
il_buyer = il.buyer,
il_recipient = il.recipient,
il_verified_flag = il.verified_flag,
il_processed_flag = il.processed_flag,
il_cogs_markup_amount = il.cogs_markup_amount,
il_sales_discount_group_id = il.sales_discount_group_id,
il_price_family_uid = il.price_family_uid,
il_sent_to_carrier_date = il.sent_to_carrier_date,
il_discount_item_flag = il.discount_item_flag,
il_other_charge_credit_rebill_flag = il.other_charge_credit_rebill_flag
from	invoice_line il
		join inv_mast m on il.inv_mast_uid = m.inv_mast_uid
where	il.company_id = @company_id
		and il.order_no = @order_no

-- shipment
select	p.pick_ticket_no
		,carrier_code = p.carrier_id
		,title = coalesce(csr.carrier_name, a.name)
		,tracking_number = coalesce(csr.tracking_no, sm.shipment_id, p.tracking_no)		   
from	oe_pick_ticket p
		left join clippership_return_10004 csr on csr.pick_ticket_no = p.pick_ticket_no
			and csr.delete_flag = 'N'
		left join shipment sm on p.pick_ticket_no = sm.transaction_no
			and sm.row_status_flag = 2175 -- accepted
			and sm.transaction_type_cd = 1000 -- pick ticket
		left join [address] a on a.id = coalesce(p.carrier_id, sm.carrier_id)
where	p.company_id = @company_id
		and p.order_no = @order_no
		and p.delete_flag <> 'Y'
		and isnull(csr.delete_flag, 'N') <> 'Y'

-- shipment line
select	qty = d.ship_quantity / d.unit_size
		,m.item_id  
		,p.pick_ticket_no
from	oe_pick_ticket p
		join oe_pick_ticket_detail d on p.company_id = d.company_id
			and p.pick_ticket_no = d.pick_ticket_no
		left join inv_mast m on d.inv_mast_uid = m.inv_mast_uid
where	p.company_id = @company_id
		and p.order_no = @order_no
		and p.delete_flag <> 'Y'
";
                #endregion

                try
                {
                    using (var connection = DbConn.GetP21DbConnection())
                    {
                        using (var multi = connection.QueryMultiple(sql, new { company_id, order_no }))
                        {
                            var SForder = multi.Read<SalesForceOrder>().FirstOrDefault();
                            if (SForder == null) return null;
                            SForder.SFlines = multi.Read<SalesForceOrderLine>().ToList();
                            SForder.SFinvoices = multi.Read<SalesForceOrderInvoice>().ToList();
                            //Logger.LogDebug(JsonConvert.SerializeObject(order));
                            var invoiceLines = multi.Read<SalesForceOrderInvoiceLine>().ToList();
                            //Logger.LogDebug(JsonConvert.SerializeObject(invoiceLines));
                            foreach (var sinvoice in SForder.SFinvoices)
                            {
                                sinvoice.items = invoiceLines
                                    .Where(_ => _.il_invoice_no == sinvoice.ih_invoice_no)
                                    .ToList();
                            }
                            SForder.SFshipments = multi.Read<SalesForceOrderShipment>().ToList();
                            var shipmentLines = multi.Read<SalesForceOrderShipmentLine>().ToList();
                            foreach (var shipment in SForder.SFshipments)
                            {
                                shipment.items = shipmentLines
                                    .Where(_ => _.pick_ticket_no == shipment.pick_ticket_no)
                                    .ToList();
                            }

                            return SForder;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.ToString());
                }
                return null;
            }
        }

        public InvoiceDetails GetInvoiceInfo(string invoice_no)
        {
            #region sql
            string sql = @"select 
	h.invoice_no
	,oe.pick_ticket_no
	,h.invoice_date
	,h.net_due_date
	,h.po_no
	,o.order_no
	,b.branch_description
	,b.branch_id
	,company_address1 = ad.mail_address1
	,company_country = ad.mail_country
	,company_city = ad.mail_city
	,company_state = ad.mail_state
    ,customer_company_name=ad.name
	,company_postal_code = ad.mail_postal_code
	,bill2_address1 = h.bill2_address1
	,bill2_address2 = h.bill2_address2
	,bill2_address3 = h.bill2_address3
	,ship2_address1 = h.ship2_address1
	,ship2_address1 = h.ship2_address2
	,ship2_address1 = h.ship2_address3
	,bill2_city = h.bill2_city
	,bill2_country = h.bill2_country
	,bill2_postal_code = h.bill2_postal_code
	,bill2_state = h.bill2_state
	,ship2_city = h.ship2_city
	,ship2_country = h.ship2_country
	,ship2_state = h.ship2_state
	,ship2_postal_code = h.ship2_postal_code
	,h.terms_desc
	,h.terms_due_date
	,h.terms_amount
	,o.taker
	,h.salesrep_id
	,salesrep_name = coalesce(h.salesrep_name,Concat(s.first_name, ' ',s.last_name))
	,h.salesrep_name
	,oe.carrier_id
	,oe.tracking_no
	,oe.direct_shipment
	,h.carrier_name
    ,h.order_date
    ,h.amount_paid
	

from 
	invoice_hdr as h 
	inner join oe_hdr o 
		on h.order_no = o.order_no  
	inner join branch b
		on h.branch_id = b.branch_id
	inner join oe_pick_ticket oe
		on oe.invoice_no = h.invoice_no
	inner join address ad
		on ad.address_id_string = '100001'
	inner join contacts s
		on h.salesrep_id = s.id
where 
	h.invoice_no = @invoice_no

select line.extended_price
	,line.inv_mast_uid
	,line.unit_price
	,line.item_desc
	,line.item_id
	,line.unit_of_measure
	,remaining = (line.qty_requested - line.qty_shipped)
	,ordered=line.qty_requested
    ,line.invoice_line_type
	,shipped=line.qty_shipped from  invoice_line line 
		where line.invoice_no = @invoice_no";
            InvoiceDetails invoiceDetails = new InvoiceDetails();
            using (var conn = DbConn.GetP21DbConnection())
            {
                try
                {
                    conn.Open();
                    var multi = conn.QueryMultiple(sql, new
                    {
                        invoice_no
                    });
                    invoiceDetails.invoiceInfo = multi.Read<InvoiceInfo>().FirstOrDefault();
                    invoiceDetails.invoiceLines = multi.Read<InvoiceLines>();
                    return invoiceDetails;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            #endregion
            return null;
        }


        public OrderDetails GetOrderInfo(string order_no)
        {
            #region sql
            string sql = @"select 
                           oe_hdr.order_no
                          ,q.expiration_date
                          ,status = (case when oe_hdr.completed = 'Y' THEN 'COMPLETED' WHEN oe_hdr.cancel_flag = 'Y' THEN 'REJECTED' WHEN oe_hdr.approved = 'Y' THEN 'APPROVED' ELSE 'PENDING' END)
                          ,oe_hdr.order_date
                          ,address.mail_address1
                          ,address.mail_address2
                          ,address.mail_address3
                          ,address.mail_city
                          ,address.mail_country
                          ,address.mail_postal_code
                          ,address.mail_state
                          ,address.email_address
                          ,address.central_phone_number
                          ,sub_total = (
                                    select 
                                        sum(extended_price) 
                                    from 
                                        oe_line 
                                    where 
                                        oe_line.order_no = oe_hdr.order_no
                                    )
                          ,shipping_method = (
		                        select 
			                        name 
		                        from address 
		                        where 
			                        address.id = oe_hdr.carrier_id
		                        )
                          ,tax = (
		                        select 
			                        sum(tax_percentage) 
		                        from tax_jurisdiction 
		                        where 
			                        jurisdiction_id 
		                        in (
			                        select 
				                        jurisdiction_id 
			                        from oe_hdr_tax 
			                        where 
				                        order_no = oe_hdr.order_no
			                        )
		                        )*100
  
                          ,oe_hdr.cancel_flag
                          ,oe_hdr.freight_out
                          ,oe_hdr.ship2_add1
                          ,oe_hdr.ship2_add2
                          ,oe_hdr.ship2_add3
                          ,oe_hdr.ship2_city
                          ,oe_hdr.ship2_country
                          ,oe_hdr.ship2_email_address
                          ,oe_hdr.ship2_name
                          ,oe_hdr.ship2_state
                          ,oe_hdr.ship2_zip
                          ,oe_hdr.ship_to_phone
                          ,oe_hdr.carrier_id
                          ,address.name
                          ,oe_hdr.po_no
                          ,ar_payment_details.payment_desc
                          ,ar_payment_details.payment_type_id
                          ,shipping_cost = (
                                     select 
                                        sum(freight_out) 
                                     from 
                                        oe_pick_ticket
                                     where order_no = oe_hdr.order_no
                                )
                          from oe_hdr  
                          left join 
		                        address 
	                        on 
		                        oe_hdr.customer_id = address.id 
                          left join 
		                        remittances 
	                        on 
		                        remittances.order_no = oe_hdr.order_no 
                          left join 
		                        ar_payment_details 
	                        on 
		                        remittances.payment_number = ar_payment_details.payment_number
                          left join 
                                quote_hdr q
                            on 
                                oe_hdr.oe_hdr_uid = q.oe_hdr_uid
                          where 
	                        oe_hdr.order_no = @order_no
	
                        select inv_mast.item_id
                          ,inv_mast.item_desc
                          ,inv_mast.inv_mast_uid
                          ,oe_line.qty_ordered
                          ,oe_line.qty_allocated
                          ,oe_line.qty_invoiced
                          ,oe_line.qty_canceled
                          ,qty_open = (oe_line.qty_ordered - oe_line.qty_invoiced - oe_line.qty_canceled) / COALESCE (NULLIF (oe_line.unit_size, 0), 1)
                          ,oe_line.pricing_unit
                          ,oe_line.pricing_unit_size
                          ,oe_line.unit_price
                          ,oe_line.extended_price
                          ,oe_line.unit_of_measure
                          ,oe_line.unit_size
                          ,oe_line.disposition
                          ,oe_line_promise_date.promise_date
                          ,oe_line.expedite_date
                          ,oe_line.required_date
                        from 
	                        oe_line
                        left join
                                oe_line_promise_date
                            on 
                                oe_line.oe_line_uid = oe_line_promise_date.oe_line_uid
                        inner join 
		                        inv_mast 
	                        on 
		                        inv_mast.inv_mast_uid = oe_line.inv_mast_uid 
                        where 
	                        order_no = @order_no";
            #endregion
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    var multi = conn.QueryMultiple(sql, new { order_no });
                    OrderDetails data = multi.Read<OrderDetails>().FirstOrDefault();
                    if (data != null)
                    {
                        Logger.LogDebug("data" + JsonConvert.SerializeObject(data));
                        data.orderLines = multi.Read<OrderLines>();
                        return data;
                    }


                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
            return null;
        }

        public IEnumerable<Order_Header> GetCustomerOrderDetails(OfflineQuoteFilterParam order_header)
        {
            #region sales_order_sql
            string sql = @"
;with orders as (select 
                           ship2_id=oe_hdr.address_id,oe_hdr.ship2_name,
                           status = (case when oe_hdr.completed = 'Y' THEN 'COMPLETED' WHEN oe_hdr.cancel_flag = 'Y' THEN 'REJECTED' WHEN oe_hdr.approved = 'Y' THEN 'APPROVED' ELSE 'PENDING' END),
                           oe_hdr.freight_out,
                           oe_hdr.po_no,
                           oe_hdr.order_no,
                           oe_hdr.order_date,
                           q.expiration_date,
                           oe_hdr.date_created,
                           oe_hdr.web_reference_no,
                           oe_hdr.delivery_instructions,
                           a.name,
lines_count=(select count(*) from oe_line where delete_flag<>'Y' and order_no=oe_hdr.order_no)
                           ,sub_total = (
                                    select 
                                        sum(extended_price) 
                                    from 
                                        oe_line 
                                    where 
                                        oe_line.order_no = oe_hdr.order_no
                                    ), 
                          tax = (
                                    select 
                                        sum(tax_percentage) 
                                    from 
                                        tax_jurisdiction 
                                    where 
                                        jurisdiction_id in 
                                              (
                                                select 
                                                    jurisdiction_id 
                                                from 
                                                    oe_hdr_tax 
                                                where order_no = oe_hdr.order_no
                                             )
                                ),
                        shipping_cost = (
                                     select 
                                        sum(freight_out) 
                                     from 
                                        oe_pick_ticket
                                     where order_no = oe_hdr.order_no
                                )
                        from 
                            oe_hdr 
                         left join contacts c on c.id = contact_id
                        left join address a on a.id = c.address_id
                        left join quote_hdr q on oe_hdr.oe_hdr_uid = q.oe_hdr_uid
                        where  {{order_type}}   {{customer_id}} {{contact_id}} )

select ship2_id,ship2_name,
       status,
       freight_out,
       po_no,
       order_no,
       order_date,
web_reference_no,
       expiration_date,
       date_created,
       name,
       sub_total,
       tax,
       shipping_cost,
lines_count,
delivery_instructions,
       total_records = cnt
from orders
                cross join (select cnt = count(1) 
from orders     Where {{Filters}} and {{type_filters}} and {{order_filter}}) as cnt
                Where {{Filters}} and {{type_filters}} and {{order_filter}}
                order by {{order_by}} {{sort_order}}
                OFFSET ((@page_number-1) * @page_size) ROWS
                FETCH NEXT @page_size ROWS ONLY";
            //  and oe_hdr.projected_order <> 'Y'  
            //  (oe_hdr.web_reference_no is null or oe_hdr.web_reference_no = '')
            sql = ApplyFiltersForQuoteService(sql, order_header);
            #endregion
            //            #region quote_sql
            //            string quote_sql = @"
            //;with orders as(SELECT po_no
            //      ,order_no
            //      ,customer_id
            //      ,company_id
            //      ,order_date
            //      ,item_id
            //      ,item_description
            //      ,qtyorder
            //      ,qtyopen
            //      ,unit_of_measure
            //      ,est_date
            //      ,line_no
            //      ,promise_date
            //      ,inv_mast_uid
            // FROM dbo.get_backorders
            // WHERE customer_id = @customer_id)
            //select po_no
            //      ,order_no
            //      ,customer_id
            //      ,company_id
            //      ,order_date
            //      ,item_id
            //      ,item_description
            //      ,qtyorder
            //      ,qtyopen
            //      ,unit_of_measure
            //      ,est_date
            //      ,line_no
            //      ,promise_date
            //      ,inv_mast_uid
            //      ,total_records = cnt
            //from orders
            //                cross join (select cnt = count(1) from orders Where {{Filters}} and {{type_filters}} and {{order_filter}}) as cnt
            //                Where {{Filters}} and {{type_filters}} and {{order_filter}}
            //                order by {{order_by}} {{sort_order}}
            //                OFFSET ((@page_number-1) * @page_size) ROWS
            //                FETCH NEXT @page_size ROWS ONLY";
            //            quote_sql = ApplyFiltersForQuoteService(quote_sql, order_header);
            //            #endregion
            Logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(order_header));
            // Logger.LogDebug(order_header.customer_id);
            if (order_header.customer_id != null)
            { sql = sql.Replace("{{customer_id}}", " and oe_hdr.customer_id = @customer_id "); }
            else { sql = sql.Replace("{{customer_id}}", ""); }

            if (order_header.contact_id != null && order_header.contact_id != 0)
            { sql = sql.Replace("{{contact_id}}", " and oe_hdr.contact_id = @contact_id "); }
            else { sql = sql.Replace("{{contact_id}}", ""); }
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();

                    var order_details = conn.Query<Order_Header>(sql,
                        new
                        {
                            order_header.customer_id,
                            order_header.contact_id,
                            order_header.from_date,
                            order_header.to_date,
                            order_header.page_number,
                            order_header.page_size,
                            order_header.filter_value,
                            order_header.order_ids
                        });
                    if (order_details == null) return null; foreach (var d in order_details)
                    {
                        d.total = d.sub_total + (d.sub_total * d.tax);
                    }

                    return order_details;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }

            return null;
        }
        public string ApplyFiltersForQuoteService(string sql, OfflineQuoteFilterParam param)
        {
            bool filter = true;
            string filter_applied_sql = sql;
            bool type_filter_applied = false;
            bool date_range_filter_applied = false;
            bool order_ids_filter_applied = false;
            bool field_filter_applied = false;
            foreach (var item in param.filter_by)
            {
                if (filter)
                {
                    switch (item)
                    {
                        case "field":
                            field_filter_applied = true;
                            filter_applied_sql = param.filter_value != null && param.filter_value != "" ? filter_applied_sql.Replace("{{Filters}}", "po_no=@filter_value or order_no=@filter_value")
                                                    : filter_applied_sql.Replace("Where {{Filters}}", "");
                            filter = false;
                            break;
                        case "days":
                            date_range_filter_applied = true;
                            filter_applied_sql = param.filter_date_range_days > 0 ? filter_applied_sql.Replace("{{Filters}}", "order_date >= '" + DateTime.Now.AddDays(0 - param.filter_date_range_days).ToString() + "'")
                                                    : filter_applied_sql.Replace("{{Filters}}", "");
                            break;
                        case "date_range":
                            date_range_filter_applied = true;
                            filter_applied_sql = param.from_date != null && param.to_date != null ? filter_applied_sql.Replace("{{Filters}}", "order_date between @from_date and @to_date")
                                                    : filter_applied_sql.Replace("{{Filters}}", "");
                            break;
                        case "MultipleOrders":
                            order_ids_filter_applied = true;
                            filter_applied_sql = param.order_ids.Count > 0 ? filter_applied_sql.Replace("{{order_filter}}", "order_no in @order_ids") :
                                                    filter_applied_sql.Replace("and {{order_filter}}", "");
                            break;
                        case "type":
                            type_filter_applied = true;
                            switch (param.filter_type)
                            {
                                case "completed":
                                    filter_applied_sql = filter_applied_sql.Replace("{{type_filters}}", "status = 'COMPLETED'");
                                    break;
                                case "approved":
                                    filter_applied_sql = filter_applied_sql.Replace("{{type_filters}}", "status = 'APPROVED'");
                                    break;
                                case "rejected":
                                    filter_applied_sql = filter_applied_sql.Replace("{{type_filters}}", "status = 'REJECTED'");
                                    break;
                                case "pending":
                                    filter_applied_sql = filter_applied_sql.Replace("{{type_filters}}", "status = 'PENDING'");
                                    break;
                                default:
                                    filter_applied_sql = filter_applied_sql.Replace("and {{type_filters}}", "");
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            string sortorder = param.sort_by;

            Logger.LogDebug("sortorder", sortorder);


            filter_applied_sql = string.IsNullOrEmpty(sortorder) ? filter_applied_sql.Replace("{{order_by}}", "order_no") : filter_applied_sql.Replace("{{order_by}}", sortorder);



            filter_applied_sql = filter_applied_sql.Replace("{{sort_order}}", param.sort_order);
            if ((!field_filter_applied && !date_range_filter_applied && !type_filter_applied && !order_ids_filter_applied))
                filter_applied_sql = filter_applied_sql.Replace("Where {{Filters}} and {{type_filters}} and {{order_filter}}", "");
            else if (!(field_filter_applied && date_range_filter_applied))
            {
                if (type_filter_applied)
                    filter_applied_sql = filter_applied_sql.Replace("{{Filters}} and ", "");
                else if (!(field_filter_applied || date_range_filter_applied))
                    filter_applied_sql = filter_applied_sql.Replace("{{Filters}} and {{type_filters}} and", "");
                else
                    filter_applied_sql = filter_applied_sql.Replace("and {{type_filters}}", "");
            }
            if (!order_ids_filter_applied)
                filter_applied_sql = filter_applied_sql.Replace("and {{order_filter}}", "");
            Logger.LogDebug((filter_applied_sql));

            Logger.LogDebug((param.order_type));
            switch (param.order_type)
            {
                case "sales":
                    filter_applied_sql = filter_applied_sql.Replace("{{order_type}}", " oe_hdr.projected_order <> 'Y'");
                    break;
                case "quote":
                    filter_applied_sql = filter_applied_sql.Replace("{{order_type}}", " oe_hdr.projected_order = 'Y'");
                    break;
                default:
                    filter_applied_sql = filter_applied_sql.Replace("{{order_type}}", "");
                    break;

            }

            Logger.LogDebug((filter_applied_sql));

            return filter_applied_sql;
        }




    }
}

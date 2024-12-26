using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("cust_defaults")]
    public class CustDefault
    {
        public string ar_account_no { get; set; }
        public string revenue_account_no { get; set; }
        public string cos_account_no { get; set; }
        public string allowed_account_no { get; set; }
        public string terms_account_no { get; set; }
        public string freight_account_no { get; set; }
        public string brokerage_account_no { get; set; }
        public decimal? price_file_id { get; set; }
        [ExplicitKey]
        public string company_id { get; set; }
        public decimal? location_id { get; set; }
        public string delete_flag { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
        public string last_maintained_by { get; set; }
        public string fob { get; set; }
        public string default_disposition { get; set; }
        public string deferred_revenue_account_no { get; set; }
        public string pick_ticket_type { get; set; }
        public string finance_chg_revenue_account_no { get; set; }
        public string invoice_type { get; set; }
        public decimal acceptable_wait_time { get; set; }
        public decimal credit_limit { get; set; }
        public string pending_payment_account_no { get; set; }
        public string terms_id { get; set; }
        public byte? pricing_method_cd { get; set; }
        public int? source_price_cd { get; set; }
        public decimal? multiplier { get; set; }
        public string price_library_id { get; set; }
        public string salesrep_id { get; set; }
        public string default_branch { get; set; }
        public decimal? preferred_location_id { get; set; }
        public int? customer_type_uid { get; set; }
        public string allow_advance_billing { get; set; }
        public string advance_bill_account_no { get; set; }
        public string signature_required { get; set; }
        public int? data_identifier_group_uid { get; set; }
        public int? cost_center_tracking_option { get; set; }
        public string packing_basis { get; set; }
        public decimal invoice_surcharge_pct { get; set; }
        public string credit_status_id { get; set; }
        public decimal? labor_rate { get; set; }
        public string taxable_flag { get; set; }
        public string allow_line_item_freight_flag { get; set; }
        public decimal? carrier_id { get; set; }
        public int? shipping_route_uid { get; set; }
        public string service_terms_id { get; set; }
        public decimal downpayment_percentage { get; set; }
        public string req_pymt_upon_release_of_items { get; set; }
        public string include_dp_summary_on_invoices { get; set; }
        public string job_number_required_flag { get; set; }
        public string use_last_margin_pricing_flag { get; set; }
        public int invoice_comp_cost_cd_tier1 { get; set; }
        public int? days_until_quote_expires { get; set; }
        public int invoice_comp_cost_cd_tier2 { get; set; }
        public int invoice_comp_cost_cd_tier3 { get; set; }
        public string cons_inv_summary_filename { get; set; }
        public string cons_inv_detail_filename { get; set; }
        public string invoice_filename { get; set; }
        public string statement_filename { get; set; }
        public int? customer_type_cd { get; set; }
        public string use_vendor_item_terms_flag { get; set; }
        public string rma_revenue_account_no { get; set; }
        public string dealer_wrrty_claims_account_no { get; set; }
        public string ar_batch_type { get; set; }
        public string cons_backorders_flag { get; set; }
    }
}

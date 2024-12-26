using P21_latest_template.Entities;

namespace P21_latest_template.Helpers
{
    public static class CustomerHelper
    {
        public static void SetDefaultValues(this Customer customer,
            Company company, CustDefault custDefault)
        {
            customer.admin_fee_flag = "N";
            customer.allow_exceed_job_qty = "N";
            customer.allow_item_level_contract_flag = "N";
            customer.allow_line_item_freight_flag = "N";
            customer.allow_non_job_item = "N";
            customer.always_use_job_price = "N";
            customer.accept_interchangeable_items = "N";
            customer.accept_partial_orders = "N";
            customer.acceptable_wait_time = custDefault.acceptable_wait_time;
            customer.allowed_account_no = custDefault.allowed_account_no;
            customer.ar_account_no = custDefault.ar_account_no;
            customer.ar_batch_type = "C";
            customer.billed_on_gross_net_qty = "G";
            customer.brokerage_account_no = custDefault.brokerage_account_no;
            customer.cos_account_no = custDefault.cos_account_no;
            customer.cod_required_flag = "N";
            customer.ci_for_complete_orders_only = "N";
            customer.ci_print_detail = "N";
            customer.credit_limit = custDefault.credit_limit;
            customer.credit_limit_check_at_shipment = "N";
            customer.credit_limit_per_order = 0m;
            customer.credit_limit_used = 0m;
            customer.currency_id = company.home_currency_id;
            customer.customer_id_string = customer.customer_id.ToString("G29");
            customer.customer_type_cd = 1203;
            customer.days_until_quote_expires = custDefault.days_until_quote_expires;
            customer.default_disposition = custDefault.default_disposition;
            customer.default_orders_to_will_call = "N";
            customer.deferred_revenue_account_no = custDefault.deferred_revenue_account_no;
            customer.downpayment_percentage = custDefault.downpayment_percentage;
            customer.environmental_fee_flag = "N";
            customer.exclude_canceld_from_pack_list = "Y";
            customer.exclude_canceld_from_order_ack = "N";
            customer.exclude_canceld_from_pick_tix = "Y";
            customer.exclude_hold_from_order_ack = "N";
            customer.exclude_hold_from_pack_list = "N";
            customer.exclude_hold_from_pick_tix = "N";
            customer.fascor_wms_pricing_option = 200;
            customer.fc_grace_days = company.fc_grace_days.HasValue ? company.fc_grace_days.Value : 0m;
            customer.fc_percentage = company.fc_percentage.HasValue ? company.fc_percentage.Value : 0m;
            customer.finance_chg_revenue_account_no = custDefault.finance_chg_revenue_account_no;
            customer.floor_plan_account = "N";
            customer.fob_required_flag = "Y";
            customer.freight_account_no = custDefault.freight_account_no;
            customer.generate_customer_statements = "Y";
            customer.generate_statements_by = 1005;
            customer.generate_finance_charges = company.generate_finance_charges;
            customer.highest_credit_limit_used = 0m;
            customer.include_dp_summary_on_invoices = custDefault.include_dp_summary_on_invoices;
            customer.include_non_alloc_on_pack_list = 1279;
            customer.include_non_alloc_on_pick_tix = 1278;
            customer.invoice_batch_uid = 1;
            customer.invoice_comp_cost_cd_tier1 = custDefault.invoice_comp_cost_cd_tier1;
            customer.invoice_comp_cost_cd_tier2 = custDefault.invoice_comp_cost_cd_tier2;
            customer.invoice_comp_cost_cd_tier3 = custDefault.invoice_comp_cost_cd_tier3;
            customer.invoice_print_qty = 1;
            customer.job_number_required_flag = custDefault.job_number_required_flag;
            customer.job_pricing = "N";
            customer.lot_bill_summary_on_invoices = "Y";
            customer.limit_max_shipments_per_order = 0m;
            customer.minimum_finance_charge = company.minimum_fc.HasValue ? company.minimum_fc.Value : 0m;
            customer.minimum_order_dollar_amount = 0m;
            customer.open_item_balance_forward = "O";
            customer.order_acknowledgments = "Y";
            customer.override_profit_limit = "N";
            customer.override_revenue_by_item = "N";
            customer.pedigree_customer = "N";
            customer.po_no_required = "N";
            customer.pick_ticket_type = custDefault.pick_ticket_type;
            customer.preferred_flag = "N";
            customer.pricing_method_cd = custDefault.pricing_method_cd;
            customer.print_lot_attrib_on_invoice = "N";
            customer.print_lot_attrib_on_packlist = "N";
            customer.print_packinglist_in_shipping = "N";
            customer.print_prices_on_packinglist = "N";
            customer.print_zero_dollar_customers = "N";
            customer.prompt_for_non_job_item = "N";
            customer.req_pymt_upon_release_of_items = custDefault.req_pymt_upon_release_of_items;
            customer.revenue_account_no = custDefault.revenue_account_no;
            customer.ship_to_credit_limit = 0;
            customer.source_price_cd = custDefault.source_price_cd;
            customer.source_type_cd = 1376;
            customer.so_po_no_required_flag = "N";
            customer.special_labeling_flag = "N";
            customer.special_packaging_flag = "N";
            customer.statement_batch_uid = 1;
            customer.terms_account_no = custDefault.terms_account_no;
            customer.terms_id = custDefault.terms_id;
            customer.trade_percent_disc = 0;
            customer.ups_handling_charge = 0;
            customer.use_consolidated_invoicing = "N";
            customer.use_last_margin_pricing_flag = custDefault.use_last_margin_pricing_flag;
            customer.use_sys_ups_handling_chrg_flag = "N";
            customer.salesrep_id = custDefault.salesrep_id;
            customer.web_enabled_flag = "N";
            customer.pending_payment_account_no = custDefault.pending_payment_account_no;
            customer.default_branch_id = custDefault.default_branch;
            customer.signature_required_flag = custDefault.signature_required ?? "N";
            customer.taxable_flag = custDefault.taxable_flag;
            customer.edi_or_paper = "P";
            customer.allow_advance_billing = custDefault.allow_advance_billing;
            customer.disp_addl_info_on_invc_flag = "N";
            customer.cust_part_no_group_hdr_uid = 0;
            customer.send_dsc856_flag = "N";
            customer.promise_date_buffer = 0;
            customer.clock_cell_tracking_flag = "N";
            customer.epa_cert_on_file_flag = "N";
            customer.order_disc_type = 230;
            customer.order_disc_factor = 0;
            customer.use_vendor_contracts_flag = "Y";
            customer.national_account_flag = "N";
            customer.price_label_flag = "N";
            customer.use_int_address_format_flag = "N";
            customer.gl_code_override_flag = "N";
            customer.price_rounding_flag = "N";
            customer.apply_convenience_fee_flag = "N";

        }
    }
}

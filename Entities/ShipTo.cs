using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Entities
{
    [Table("ship_to")]
    public class ShipTo
    {
        [ExplicitKey]
        public string company_id { get; set; }
        [ExplicitKey]
        public decimal ship_to_id { get; set; }
        public decimal customer_id { get; set; }
        public string default_branch { get; set; }
        public string federal_exemption_number { get; set; }
        public string state_exemption_number { get; set; }
        public string other_exemption_number { get; set; }
        public decimal? price_file_id { get; set; }
        public decimal? preferred_location_id { get; set; }
        public DateTime? default_ship_time { get; set; }
        public decimal? default_carrier_id { get; set; }
        public string fob { get; set; }
        public string delivery_instructions { get; set; }
        public DateTime? morning_beg_delivery { get; set; }
        public DateTime? morning_end_delivery { get; set; }
        public DateTime? evening_beg_delivery { get; set; }
        public DateTime? evening_end_delivery { get; set; }
        public string accept_partial_orders { get; set; }
        public decimal? acceptable_wait_time { get; set; }
        public string class1_id { get; set; }
        public string class2_id { get; set; }
        public string class3_id { get; set; }
        public string class4_id { get; set; }
        public string class5_id { get; set; }
        public string packing_basis { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
        public string last_maintained_by { get; set; }
        public string delete_flag { get; set; }
        public string terms_id { get; set; }
        public string invoice_type { get; set; }
        public string billed_on_gross_net_qty { get; set; }
        public string state_excise_tax_exemption_no { get; set; }
        public string pick_ticket_type { get; set; }
        public string tax_group_id { get; set; }
        public string handling_charge_req_flag { get; set; }
        public string third_party_billing_flag { get; set; }
        public int? days_early { get; set; }
        public int? days_late { get; set; }
        public int? transit_days { get; set; }
        public int? freight_code_uid { get; set; }
        public string signature_required { get; set; }
        public int? shipping_route_uid { get; set; }
        public string pda_order_entry { get; set; }
        public int? pda_oelist_criteria_uid { get; set; }
        public string exclude_hold_from_pick_tix { get; set; }
        public string exclude_canceld_from_pack_list { get; set; }
        public string exclude_hold_from_pack_list { get; set; }
        public string exclude_canceld_from_order_ack { get; set; }
        public string exclude_hold_from_order_ack { get; set; }
        public int include_non_alloc_on_pick_tix { get; set; }
        public string exclude_canceld_from_pick_tix { get; set; }
        public string created_by { get; set; }
        public int include_non_alloc_on_pack_list { get; set; }
        public string print_packinglist_in_shipping { get; set; }
        public string print_prices_on_packinglist { get; set; }
        public int? source_type_cd { get; set; }
        public int? data_identifier_group_uid { get; set; }
        public decimal? fedex_freight_markup { get; set; }
        public string credit_status { get; set; }
        public decimal? credit_limit { get; set; }
        public decimal? credit_limit_used { get; set; }
        public string budget_code_approval_flag { get; set; }
        public string service_terms_id { get; set; }
        public string default_customer_po_no { get; set; }
        public decimal default_freight_markup_pct { get; set; }
        public string drum_deposit_exempt_flag { get; set; }
        public string taxable_flag { get; set; }
        public string bill_hold_flag { get; set; }
        public string display_po_no_flag { get; set; }
        public string po_no_required_flag { get; set; }
        public string display_badge_flag { get; set; }
        public string badge_required_flag { get; set; }
        public string dfoa_ship_to_id { get; set; }
        public string erouter_tran_type { get; set; }
        public int? freight_charge_by_mile_hdr_uid { get; set; }
        public decimal? freight_mileage_amt { get; set; }
        public string degree_days_flag { get; set; }
        public string calendar_days_flag { get; set; }
        public int? heat_and_hot_water_cd { get; set; }
        public decimal? del_sch_source_location { get; set; }
        public decimal? service_source_location { get; set; }
        public string distributor_account_id { get; set; }
        public string cardlock_calc_price_flag { get; set; }
        public decimal? courtesy_address_id { get; set; }
        public decimal? cardlock_discount { get; set; }
        public int? order_priority_uid { get; set; }
        public int? delivery_zone_uid { get; set; }
        public int? freight_charge_uid { get; set; }
        public string plant_code { get; set; }
        public string ups_roadnet_acct_type_id { get; set; }
        public string ups_roadnet_delivery_days { get; set; }
        public string ups_roadnet_zone_id { get; set; }
        public string use_cust_ups_handlng_chrg_flag { get; set; }
        public decimal ups_handling_charge { get; set; }
        public string sales_tax_payable_account_no { get; set; }
        public string vertex_taxable_flag { get; set; }
        public string customer_tax_class { get; set; }
        public int? tax_exempt_reason_uid { get; set; }
        public int? service_center_uid { get; set; }
        public string duns_number { get; set; }
        public string small_truck_flag { get; set; }
        public int? delivery_time_offset { get; set; }
        public int? terms_of_delivery_cd { get; set; }
        public int? mode_of_transport_cd { get; set; }
        public string ups_roadnet_exclude_flag { get; set; }
        public string dfoa_sold_to_id { get; set; }
        public int? invoice_batch_uid { get; set; }
        public DateTime? date_acct_opened { get; set; }
        public decimal? courtesy_contract_ship_to_id { get; set; }
        public string ups_third_party_billing_no { get; set; }
        public string req_lot_doc_with_invoice_flag { get; set; }
        public string req_lot_doc_with_packinglist_flag { get; set; }
        public string limit_online_warranties_flag { get; set; }
        public decimal? remote_margin { get; set; }
        public string cfn_resale_flag { get; set; }
        public string separate_invoice_flag { get; set; }
        public string do_not_auto_invoice_flag { get; set; }
        public decimal? sic_code { get; set; }
        public string valvoline_number { get; set; }
        public string conoco_ship_to_id { get; set; }
        public string conoco_sold_to_id { get; set; }

        public ShipTo()
        {
            date_created = DateTime.Now;
            created_by = "admin";
            date_last_modified = DateTime.Now;
            last_maintained_by = "admin";
            delete_flag = "N";
            handling_charge_req_flag = "N";
            third_party_billing_flag = "S";
            signature_required = "N";
            pda_order_entry = "N";
            exclude_hold_from_pick_tix = "N";
            exclude_canceld_from_pack_list = "N";
            exclude_hold_from_pack_list = "N";
            exclude_canceld_from_order_ack = "N";
            exclude_hold_from_order_ack = "N";
            include_non_alloc_on_pick_tix = 1277;
            exclude_canceld_from_pick_tix = "N";
            //include_non_alloc_on_pack_list
            print_packinglist_in_shipping = "N";
            print_prices_on_packinglist = "Y";
            default_freight_markup_pct = 0;
            use_cust_ups_handlng_chrg_flag = "Y";
            ups_handling_charge = 0.00m;
            ups_roadnet_exclude_flag = "N";
            cfn_resale_flag = "N";
            separate_invoice_flag = "N";
            do_not_auto_invoice_flag = "N";
            accept_partial_orders = "Y";
            acceptable_wait_time = 0;
            invoice_type = "IN";
            billed_on_gross_net_qty = "G";
            pick_ticket_type = "UT";
            third_party_billing_flag = "S";
            days_early = 0;
            days_late = 0;
            transit_days = 1;
            signature_required = "N";
            drum_deposit_exempt_flag = "N";
            degree_days_flag = "N";
            calendar_days_flag = "N";
            cardlock_calc_price_flag = "N";
            delivery_time_offset = 0;
            limit_online_warranties_flag = "N";

        }
    }
}

using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("address")]
    public class Address
    {
        [ExplicitKey]
        public decimal id { get; set; }
        public string name { get; set; }
        public string mail_address1 { get; set; }
        public string mail_address2 { get; set; }
        public string mail_city { get; set; }
        public string mail_state { get; set; }
        public string mail_postal_code { get; set; }
        public string mail_country { get; set; }
        public string phys_address1 { get; set; }
        public string phys_address2 { get; set; }
        public string phys_city { get; set; }
        public string phys_state { get; set; }
        public string phys_postal_code { get; set; }
        public string phys_country { get; set; }
        public string central_phone_number { get; set; }
        public string central_fax_number { get; set; }
        public string federal_id_number { get; set; }
        public string resale_certificate { get; set; }
        public string ups_code { get; set; }
        public decimal? corp_address_id { get; set; }
        public decimal? billing_address_id { get; set; }
        public decimal? credit_address_id { get; set; }
        public string customer { get; set; }
        public string vendor { get; set; }
        public string employee { get; set; }
        public string prospect { get; set; }
        public string billing_address { get; set; }
        public string shipping_address { get; set; }
        public string payment_address { get; set; }
        public string incorporated { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
        public string last_maintained_by { get; set; }
        public string delete_flag { get; set; }
        public DateTime? default_ship_time { get; set; }
        public decimal? preferred_location_id { get; set; }
        public string carrier_id { get; set; }
        public string delivery_instructions1 { get; set; }
        public string delivery_instructions2 { get; set; }
        public string delivery_instructions3 { get; set; }
        public DateTime? mor_beg_delivery { get; set; }
        public DateTime? mor_end_delivery { get; set; }
        public DateTime? eve_beg_delivery { get; set; }
        public DateTime? eve_end_delivery { get; set; }
        public string inventory_location_flag { get; set; }
        public string show_out_items { get; set; }
        public string store_no { get; set; }
        public string invoice_type { get; set; }
        public string address_id_string { get; set; }
        public string carrier_flag { get; set; }
        public decimal? default_carrier_id { get; set; }
        public decimal? trade_percent_disc { get; set; }
        public string class1_id { get; set; }
        public string class2_id { get; set; }
        public string class3_id { get; set; }
        public string class4_id { get; set; }
        public string class5_id { get; set; }
        public string central_watts_number { get; set; }
        public string alternative_1099_name { get; set; }
        public string name_control { get; set; }
        public string default_ship_to_company { get; set; }
        public string default_ship_to_branch { get; set; }
        public string ship_to_packing_basis { get; set; }
        public string bill_of_lading_type { get; set; }
        public string email_address { get; set; }
        public string url { get; set; }
        public string carrier_fedex_flag { get; set; }
        public string routeview_require_routing_flag { get; set; }
        public int carrier_type_cd { get; set; }
        public string carrier_do_not_route_flag { get; set; }
        public string roadnet_do_not_route_flag { get; set; }
        public string carrier_strategic_freight_flag { get; set; }
        public decimal? carrier_fixed_freight_charge { get; set; }
        public decimal? carrier_freight_est_percentage { get; set; }
        public int? expedited_freight_option_cd { get; set; }
        public int? order_priority_uid { get; set; }
        public string dc_do_not_route_flag { get; set; }
        public string roadnet_pt_print_option { get; set; }
        public string sfdc_account_id { get; set; }
        public DateTime? sfdc_create_date { get; set; }
        public DateTime? sfdc_update_date { get; set; }
        public decimal? carrier_fixed_freight_markup { get; set; }
        public string phys_county { get; set; }
        public string scac_code { get; set; }
        public int? freight_code_uid { get; set; }
        public int? carrier_pallet_weight_limit { get; set; }
        public decimal? carrier_pallet_freight_charge { get; set; }
        public int? carrier_transit_days { get; set; }
        public string fidelitone_carrier_id { get; set; }
        // public string ext_tax_freight_tax_code_in { get; set; }
        //  public string ext_tax_freight_tax_code_out { get; set; }
        public string mail_address3 { get; set; }
        public string phys_address3 { get; set; }

        public Address()
        {
            date_created = DateTime.Now;
            date_last_modified = DateTime.Now;
            last_maintained_by = "admin";
            delete_flag = "N";
            carrier_fedex_flag = "N";
            routeview_require_routing_flag = "N";
            carrier_type_cd = 300;
            carrier_do_not_route_flag = "N";
        }
    }
}

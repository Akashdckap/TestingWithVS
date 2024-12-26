using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("contacts")]
    public class Contact
    {
        [ExplicitKey]
        public string id { get; set; }
        public decimal address_id { get; set; }
        public string salutation { get; set; }
        public string first_name { get; set; }
        public string mi { get; set; }
        public string last_name { get; set; }
        public string title { get; set; }
        public string direct_phone { get; set; }
        public string phone_ext { get; set; }
        public string direct_fax { get; set; }
        public string fax_ext { get; set; }
        public string beeper { get; set; }
        public string cellular { get; set; }
        public string email_address { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
        public string last_maintained_by { get; set; }
        public string home_address1 { get; set; }
        public string home_address2 { get; set; }
        public DateTime? birthday { get; set; }
        public DateTime? anniversary { get; set; }
        public string home_phone { get; set; }
        public string email_address2 { get; set; }
        public string delete_flag { get; set; }
        public string comment_1 { get; set; }
        public string comment_2 { get; set; }
        public string comment_3 { get; set; }
        public string comment_4 { get; set; }
        public string salesrep { get; set; }
        public string buyer { get; set; }
        public string schedular { get; set; }
        public string login_id { get; set; }
        public string dealer { get; set; }
        public string url { get; set; }
        public string comments { get; set; }
        public string home_email_address { get; set; }
        public string home_fax { get; set; }
        public string class_1id { get; set; }
        public string class_2id { get; set; }
        public string class_3id { get; set; }
        public string class_4id { get; set; }
        public string class_5id { get; set; }
        public decimal? employee_vendor_id { get; set; }
        public string dear_field { get; set; }
        public string old_contact_id { get; set; }
        public string employee { get; set; }
        public string upper_combined_name { get; set; }
        public string descending_combined_name { get; set; }
        public string direct_watts_number { get; set; }
        public int? no_of_cycle_days { get; set; }
        public string mailstop { get; set; }
        public string commission_class_id { get; set; }
        public string company_id { get; set; }
        public decimal? vendor_id { get; set; }
        public string address_name { get; set; }
        public string driver { get; set; }
        public string inside_salesrep_flag { get; set; }
        public string default_branch_id { get; set; }
        public string technician { get; set; }
        public decimal? location_id { get; set; }
        public int? territory_uid { get; set; }
        public decimal? fuel_surcharge_percentage { get; set; }
        public decimal? max_fuel_charge_per_ship { get; set; }
        public string sales_manager_id { get; set; }
        public string roadnet_driver_id { get; set; }
        public string cellular_ext { get; set; }
        public int? contact_role_uid { get; set; }
        public int? contact_type_id { get; set; }
        public string sfdc_account_id { get; set; }
        public DateTime? sfdc_create_date { get; set; }
        public DateTime? sfdc_update_date { get; set; }
        public string sfdc_contact_id { get; set; }
        public string sales_agency_flag { get; set; }
        public string sales_agency_name { get; set; }
        public int delivery_output_cd { get; set; }
        public string driver_enable_gps_flag { get; set; }
        public DateTime? date_direct_fax_last_modified { get; set; }
        public string ads_user { get; set; }

        public Contact()
        {
            date_created = DateTime.Now;
            date_last_modified = DateTime.Now;
            delete_flag = "N";
            last_maintained_by = "admin";
            buyer = "N";
            delivery_output_cd = 2844;
            driver = "N";
            driver_enable_gps_flag = "N";
            inside_salesrep_flag = "N";
            no_of_cycle_days = 5;
            technician = "N";
        }
    }
}

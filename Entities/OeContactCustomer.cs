using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("oe_contacts_customer")]
    public class OeContactCustomer
    {
        [ExplicitKey]
        public string company_id { get; set; }
        [ExplicitKey]
        public decimal customer_id { get; set; }
        [ExplicitKey]
        public string contact_id { get; set; }
        public string delete_flag { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
        public string last_maintained_by { get; set; }
        public int shopper_uid { get; set; }
        public string statement_contact { get; set; }
        public string credit_card_no { get; set; }
        public string credit_card_type { get; set; }
        public string credit_card_name { get; set; }
        public DateTime? credit_card_expiration_date { get; set; }
        public string pedigree_contact { get; set; }

        public OeContactCustomer()
        {
            date_created = DateTime.Now;
            date_last_modified = DateTime.Now;
            delete_flag = "N";
            last_maintained_by = "admin";
            pedigree_contact = "N";
            shopper_uid = 0;
            statement_contact = "N";
        }
    }
}

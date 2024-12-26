using System.ComponentModel.DataAnnotations;

namespace P21_latest_template.Models
{
    public class AddressCreate
    {
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
    }
}

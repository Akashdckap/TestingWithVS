using System.ComponentModel.DataAnnotations;

namespace P21_latest_template.Models
{
    public class CustomerCreate
    {
        public string salesrep_id { get; set; }
        public decimal? customer_id { get; set; }
        [Required]
        public string company_id { get; set; }
        [Required]
        public string customer_name { get; set; }

        [Required]
        public ContactCreate Contact { get; set; } = new ContactCreate();
        [Required]
        public AddressCreate Address { get; set; } = new AddressCreate();
    }
}

using System.ComponentModel.DataAnnotations;

namespace P21_latest_template.Models
{
    public class CustomerContactCreate : ContactCreate
    {
        [Required]
        public string company_id { get; set; }
        [Required]
        public decimal customer_id { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace P21_latest_template.Models
{
    public class ContactCreate
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string title { get; set; }
        public string direct_phone { get; set; }
        [Required]
        public string email_address { get; set; }
    }
}

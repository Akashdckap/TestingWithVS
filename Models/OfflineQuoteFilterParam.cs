using P21_latest_template.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class OfflineQuoteFilterParam : FilterParam
    {
        [Required]
        public decimal? customer_id { get; set; }

        public decimal? contact_id { get; set; }
        public List<string> filter_by { get; set; }
        public string filter_type { get; set; }
        public string filter_value { get; set; }
        public int filter_date_range_days { get; set; }

        public DateTime from_date { get; set; }

        public DateTime to_date { get; set; }
        //[ValidOfflineOrderTypes(ErrorMessage = "Not a valid Order Type")]
        public string order_type { get; set; }
        public string sort_by { get; set; } = "order_no";
        public string sort_order { get; set; } = "asc";
        public List<decimal> order_ids { get; set; } = new List<decimal>();
    }
}

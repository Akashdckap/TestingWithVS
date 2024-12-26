using System.Collections.Generic;

namespace P21_latest_template.Models
{
    public class ProductFilterParam : FilterParam
    {
        public string company_id { get; set; }
        public string class_id { get; set; }
        public string product_type { get; set; } = "";
        public IEnumerable<string> class_values { get; set; }

        public decimal? customer_id { get; set; }
        public decimal? location_id { get; set; }
    }
}

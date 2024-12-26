using System;
using System.Collections.Generic;

namespace P21_latest_template.Models
{
    public class ProductInventoryParam
    {
        public IEnumerable<decimal> location_ids { get; set; }
        public DateTime date_last_modified { get; set; }
        public string product_type { get; set; }
    }
}

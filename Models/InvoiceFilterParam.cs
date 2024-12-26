using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P21_latest_template.Models
{
    public class InvoiceFilterParam
    {
        [Required]
        public string company_id { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public IEnumerable<string> order_ids { get; set; }
        public int page_number { get; set; } = 1;
        public int page_size { get; set; } = 15;
        public int line_count { get; set; } = -1;
    }
}

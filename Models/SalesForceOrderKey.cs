using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class SalesForceOrderKey
    {
        public string company_id { get; set; }
        public string order_no { get; set; }
        public int order_line_count { get; set; }
        public int Total { get; set; }
    }
}
